using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLog : MonoBehaviour, ILog
{

    public Engine engine;

    private IEngine engineInterface;

    public void Ask(int channel, string[] options)
    {
        int i = 1;
        foreach (string option in options)
        {
            Debug.Log(channel + ": (" + i++ + ") " + option);
        }
    }

    public void NotifyTuned(int channel, string frequency, int noise)
    {
        Debug.Log(channel + ": TUNED @ " + frequency + " w/ " + noise);
    }


    public void Break(int channel)
    {
        Debug.Log(channel + ": separator");
    }

    public void Send(int channel, string sentence)
    {
        Debug.Log(channel + ": " + sentence);
    }

    // Use this for initialization
    void Start()
    {
        engineInterface = engine;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void Choose(int index)
    {
        engineInterface.Answer(index);
    }

    public void Choose1()
    {
        Choose(0);
    }

    public void Choose2()
    {
        Choose(1);
    }

    public void Choose3()
    {
        Choose(2);
    }

    public void Choose4()
    {
        Choose(3);
    }

    public void Choose5()
    {
        Choose(4);
    }

    public void NotifyReady(int channel)
    {
        Debug.Log(channel + ": NEW MESSAGE");
    }

    public void LoseConnection()
    {
        Debug.Log("Connection LOST");
    }

    public void UpdateNoise()
    {
        Debug.Log("Update noise");
    }
}
