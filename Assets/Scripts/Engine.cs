using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Engine : MonoBehaviour, IEngine {

    public Canvas canvas;
    public Map map;
    public SoundManager soundManager;

    private ILog log;
    private ICurtains curtains;

    private Channel[] channels;
    private int? currentChannelIndex;
    private Vector2[] destinations;

    private bool debugMode;

	// Use this for initialization
	void Start () {
        log = canvas.GetComponentInChildren<ILog>();
        channels = GetComponentsInChildren<Channel>();
        curtains = GetComponentInChildren<ICurtains>();
        destinations = new Vector2[channels.Length];
        currentChannelIndex = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugMode = !debugMode;
            if (debugMode)
            {
                Debug.Log("DEBUG MODE on");
                Time.timeScale = 10;
            }
            else
            {
                Debug.Log("DEBUG MODE off");
                Time.timeScale = 1;
            }            
        }
        for (int i = 0; i < destinations.Length; i++)
        {
            Vector2 destination = destinations[i];
            Channel channel = channels[i];
            float step = channel.markerSpeed * Time.deltaTime;
            Vector2 currentPosition = map.MarkerPosition(i);
            if (currentPosition == Vector2.zero)
            {
                map.MoveMarker(i, destination);
                channel.DestinationReached();
            }
            else if (step != 0)
            {
                if (!map.MoveMarker(i, Vector2.MoveTowards(currentPosition, destination, step)))
                {
                    channel.DestinationReached();
                }
            }
        }
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

    public Action OnLockFrequency { get; internal set; }

    public bool TuneTo(float frequency, int noise)
    {
        int? channelFound = null;
        for (int i = 0; i < channels.Length; i++)
        {
            if (channels[i].frequency == frequency)
            {
                channelFound = i;
            }
        }
        if (channelFound == null && CurrentChannel == null)
        {
            // Still tuned to no channel            
            return false;
        }
        else if (channelFound == null && CurrentChannel != null)
        {
            Debug.Log("Unlisten current channel, tuned to no channel");
            CurrentChannel.Unlisten();
            currentChannelIndex = null;
            log.LoseConnection();
            AbateMusic();
            return false;
        }
        else if (channelFound != null && CurrentChannel == null)
        {
            Debug.Log("Tuned to new channel");
            currentChannelIndex = channelFound;
            CurrentChannel.Listen();
        }
        else if (channelFound != null && CurrentChannel != null)
        {
            if (currentChannelIndex != channelFound) // This is not supposed to happen
            {
                Debug.Log("Unlisten current channel, tuned to new channel");
                CurrentChannel.Unlisten();
                currentChannelIndex = channelFound;
                CurrentChannel.Listen();
            }
            else
            {
                // Same channel
            }
        }
        log.NotifyTuned((int) channelFound, frequency.ToString("N1"), noise);
        log.UpdateNoise();
        map.UpdateMap((int)channelFound);
        return true;
    }

    private int GetChannelIndex(Channel channel)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            if (channels[i] == channel)
            {                
                return i;
            }
        }
        return -1; // throws exception and this is OK
    }

    public void Poke(Channel channel)
    {
        Debug.Log("Poking on channel " + GetChannelIndex(channel));
        log.NotifyReady(GetChannelIndex(channel));
    }

    public void SetDestinationX(Channel channel, float x)
    {
        int i = GetChannelIndex(channel);
        destinations[i].x = x;
    }

    public void SetDestinationY(Channel channel, float y)
    {
        int i = GetChannelIndex(channel);
        destinations[i].y = y;
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

    public void StopReception()
    {
        log.Break((int) currentChannelIndex);
        AbateMusic();
    }

    public void Receive(string sentence)
    {
        log.Send((int) currentChannelIndex, sentence);
    }

    public void ReceiveOptions(string[] options)
    {
        log.Ask((int)currentChannelIndex, options);
    }

    public void Victory()
    {
        curtains.ChangeTo(Curtain.Victory);
    }

    public void Reboot()
    {
        StartCoroutine("waitAndReboot");        
    }
    
 
     IEnumerator waitAndReboot()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    internal void IntensifyMusic()
    {
        soundManager.Intensify();
    }

    internal void AbateMusic()
    {
        soundManager.Abate();
    }

    internal void LockFrequency()
    {
        OnLockFrequency();
    }
}
