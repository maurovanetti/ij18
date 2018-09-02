using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour, IEngine {

    public Canvas canvas;

    private ILog log;
    private IRadio radio;
    private IMap map;
    //private ICurtains curtains;

    private Channel[] channels;
    private int? currentChannelIndex;

	// Use this for initialization
	void Start () {
        log = canvas.GetComponentInChildren<ILog>();
        map = canvas.GetComponentInChildren<IMap>();
        channels = GetComponentsInChildren<Channel>();
        currentChannelIndex = null;
    }
	
	// Update is called once per frame
	void Update () {
		    
	}

    private Channel CurrentChannel
    {
        get
        {
            if (currentChannelIndex == null || currentChannelIndex < 0 || currentChannelIndex >= channels.Length)
            {
                return null;
            }
            return channels[(int)currentChannelIndex];
        }
    }

    public bool TuneTo(float frequency, int noise)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            if (channels[i].frequency == frequency)
            {
                if (i != currentChannelIndex)
                {
                    if (CurrentChannel != null)
                    {
                        CurrentChannel.Unlisten();
                    }
                    currentChannelIndex = i;
                    CurrentChannel.Listen();
                }

                log.NotifyTuned(i, frequency.ToString("N1"), noise);
                log.SwitchStatus(true);
                map.UpdateMap(i);
                return true;
            }
        }
        if (CurrentChannel != null)
        {
            CurrentChannel.Unlisten();
        }
        log.SwitchStatus(false);
        return false;
    }

    public void Poke(Channel channel)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            if (channels[i] == channel)
            {
                log.NotifyReady(i);
                return;
            }
        }
    }

    public bool Answer(int optionNumber)
    {
        if (CurrentChannel != null)
        {
            CurrentChannel.Answer(optionNumber);
            return true;
        }
        return false;
    }

    public void Receive(string sentence)
    {
        log.Send((int) currentChannelIndex, sentence);
    }

    public void ReceiveOptions(string[] options)
    {
        log.Ask((int)currentChannelIndex, options);
    }
}
