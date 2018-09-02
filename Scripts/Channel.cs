using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour {
    
    public TextAsset inkAsset;
    public float frequency;    
    public float tickDuration;

    private Engine engine;
    private Story story;
    private float endOfPause;
    private bool listened;
    private bool waiting;

    // Use this for initialization
    void Start () {
        engine = GetComponent<Engine>();
        story = new Story(inkAsset.text);
        endOfPause = Time.time;
        listened = false;
        waiting = true; // to prevent early notifications of readiness
    }
	
	// Update is called once per frame
	void Update () {		
        if (Time.time > endOfPause)
        {
            Broadcast();
        }
	}

    public void Listen()
    {
        listened = true;
    }

    public void Unlisten()
    {
        listened = false;
    }

    private void Broadcast()
    {                
        if (engine != null)
        {
            if (story.canContinue)
            {
                if (listened)
                {
                    waiting = false;
                    string sentence = story.Continue();
                    Pause(sentence.Length);
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
                }
                else if (!waiting)
                {
                    waiting = true;
                    engine.Poke(this);
                }
            } 
                       
        }
    }

    private void Pause(int ticks)
    {
        endOfPause = Time.time + (ticks * tickDuration);
    }

    public void Answer(int optionNumber)
    {        
        story.ChooseChoiceIndex(optionNumber);                
    }
}
