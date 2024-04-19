using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ink.Runtime;

public class Channel : MonoBehaviour {

    // tags
    public const string CanRestart = "canrestart";
    public const string VeryShortBreak = "veryshortbreak";
    public const string ShortBreak = "shortbreak";
    public const string LongBreak = "longbreak";
    public const string VeryLongBreak = "vertylongbreak";
    public const string Victory = "victory";
    public const string Reboot = "restart";
    public const string Intensify = "intensify";
    public const string Abate = "abate";

    public const string InterChannelVariablePrefix = "inter__";

    public TextAsset[] inkAsset;
    public float frequency;    
    public float tickDuration;
    public float markerSpeed;
    public bool canBlink;

    private bool canRestart;
    private Engine engine;
    private Story story;
    private float endOfPause;
    public enum State
    {
        Idle,
        NewMessage,
        Connected
    }
    private State state;
    private List<string> varsToSync = new List<string>();

    private static Dictionary<string, object> interChannelVariables = new Dictionary<string, object>();
    private bool destinationReached;
    private bool startOfTransmission;
    private bool overAndOut;    

    // Use this for initialization
    void Start() {
        engine = GetComponent<Engine>();
        Initialize();
        story.ObserveVariable("x", (string varName, object newValue) => {
            DestinationNotReached();
            engine.SetDestinationX(this, (float)newValue);
        });
        story.ObserveVariable("y", (string varName, object newValue) => {
            DestinationNotReached();
            engine.SetDestinationY(this, (float)newValue);
        });
        foreach (object variable in story.variablesState)
        {
            string variableName = variable.ToString();
            if (variableName.StartsWith(InterChannelVariablePrefix))
            {
                varsToSync.Add(variableName);
                interChannelVariables[variableName] = story.variablesState[variableName];
                story.ObserveVariable(variableName, (string varName, object newValue) => {
                    interChannelVariables[variableName] = story.variablesState[variableName];
                });
            }
        }
    }

    internal void DestinationNotReached()
    {
        Debug.Log("DestinationNotReached()");
        if (this.destinationReached)
        {
            if (startOfTransmission)
            {
                startOfTransmission = false;
            }
            else
            {
                overAndOut = true;
            }            
            this.destinationReached = false;
        }        
    }

    internal void DestinationReached()
    {
        this.destinationReached = true;
    }

    private void Initialize()
    {
        story = new Story(inkAsset[Localization.LanguageIndex].text);
        endOfPause = Time.time;
        state = State.NewMessage; // to prevent early notifications of readiness
        canRestart = false;
        destinationReached = true;
        startOfTransmission = true;
        overAndOut = false;
    }
	
	// Update is called once per frame
	void Update () {		
        if (Time.time > endOfPause && destinationReached)
        {
            Broadcast();
        }
	}

    public void Listen()
    {        
        if (canRestart && !story.canContinue)
        {
            Initialize();
        }
        state = State.Connected;
    }

    public void Unlisten()
    {
        state = State.Idle;
    }

    private void Broadcast()
    {                
        if (engine != null)
        {
            if (story.canContinue)
            {                
                if (state == State.Connected)
                {                    
                    foreach (string varToSync in varsToSync)
                    {
                        story.variablesState[varToSync] = interChannelVariables[varToSync];                        
                    }
                    string sentence = story.Continue();
                    foreach (string tag in story.currentTags)
                    {
                        Debug.Log("TAG found: " + tag);
                    }
                    if (story.currentTags.Contains(CanRestart))
                    {
                        canRestart = true;
                    }
                    if (story.currentTags.Contains(Victory))
                    {
                        engine.Victory();
                    }
                    if (story.currentTags.Contains(Reboot))
                    {
                        engine.Reboot();
                    }
                    float extraPause = 0;
                    if (story.currentTags.Contains(VeryShortBreak))
                    {
                        extraPause += 4;
                    }
                    if (story.currentTags.Contains(ShortBreak))
                    {
                        extraPause += 20;
                    }
                    if (story.currentTags.Contains(LongBreak))
                    {
                        extraPause += 50;
                    }
                    if (story.currentTags.Contains(VeryLongBreak))
                    {
                        extraPause += 250;
                    }
                    if (story.currentTags.Contains(Intensify))
                    {
                        engine.IntensifyMusic();
                    }
                    if (story.currentTags.Contains(Abate))
                    {
                        engine.AbateMusic();
                    }
                    Debug.Log("Channel read: " + sentence);
                    Pause(sentence.Length, extraPause);
                    engine.Receive(sentence);
                    if (story.currentChoices.Count > 0)
                    {
                        string[] options = new string[story.currentChoices.Count];
                        int i = 0;
                        foreach (Choice choice in story.currentChoices)
                        {
                            options[i++] = choice.text;
                        }
                        engine.ReceiveOptions(options);
                    }
                    if (extraPause > 0 || overAndOut)
                    {                        
                        engine.StopReception();
                        overAndOut = false;
                    }
                }
                else if (state == State.Idle)
                {
                    state = State.NewMessage;
                    if (canBlink)
                    {
                        engine.Poke(this);
                    }
                }
            }
                       
        }
    }

    private void Pause(int ticks, float extraPause)
    {
        endOfPause = Time.time + (ticks * tickDuration) + extraPause;
    }

    public void Answer(int optionNumber)
    {        
        story.ChooseChoiceIndex(optionNumber);                
    }
}
