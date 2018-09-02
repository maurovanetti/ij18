using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Log : MonoBehaviour, ILog
{
    public Engine engine;
    public GameObject TextBlockPrefab;
    public GameObject WhiteNoise;
    public GameObject GreenLight;
    public GameObject RedLight;
    public GameObject[] TeamChannelLight;
    public GameObject[] ChannelScrollViewContent;
    private IEngine _engineInterface;

    private int _activeChannelIndex;
    private RectTransform _activeScrollViewContent;

    // Use this for initialization
    void Start()
    {
        _engineInterface = engine;
    }

    public void NotifyTuned(int channel, string frequency, int noise)
    {
        _activeChannelIndex = channel;
        _activeScrollViewContent = ChannelScrollViewContent[channel].GetComponent<RectTransform>();
    }

    public void SwitchStatus(bool connected)
    {
        WhiteNoise.SetActive(!connected);
        GreenLight.SetActive(connected);
        RedLight.SetActive(!connected);
        TeamChannelLight[_activeChannelIndex].SetActive(connected);
        ChannelScrollViewContent[_activeChannelIndex].transform.SetAsLastSibling();
        ChannelScrollViewContent[_activeChannelIndex].SetActive(connected);
    }

    public void NotifyReady(int channel)
    {
        Debug.Log(channel + ": NEW MESSAGE");
    }

    public void Send(int channel, string sentence)
    {
        GameObject textBlock = Instantiate(TextBlockPrefab, _activeScrollViewContent);
        textBlock.GetComponentInChildren<Text>().text = sentence;
        textBlock.GetComponent<RectTransform>().sizeDelta += new Vector2(0, CalcBlockHeight(sentence));
        _activeScrollViewContent.sizeDelta += new Vector2(0, textBlock.GetComponent<RectTransform>().sizeDelta.y);
        //Debug.Log(channel + ": " + sentence);
    }

    private float CalcBlockHeight(string sentence)
    {
        int heightPerLine = 60;
        int charPerLine = 60;
        float floatLines = (float)sentence.ToCharArray().Length / (float)charPerLine;
        int lines = Mathf.CeilToInt(floatLines);

        return heightPerLine * lines;
    }

    public void Ask(int channel, string[] options)
    {
        for(int i = 0; i < options.Length; i++)
        {
            GameObject answerBlock = Instantiate(TextBlockPrefab, _activeScrollViewContent);
            answerBlock.GetComponentInChildren<Text>().text = string.Format("<i>{0}</i>", options[i]);
            answerBlock.GetComponent<RectTransform>().sizeDelta += new Vector2(0, CalcBlockHeight(options[i]));
            _activeScrollViewContent.sizeDelta += new Vector2(0, answerBlock.GetComponent<RectTransform>().sizeDelta.y);
            int index = i;
            answerBlock.GetComponent<Button>().onClick.AddListener(()=> { Choose(index); });
            //Debug.Log(channel + ": (" + i++ + ") " + option);
        }
    }

    private void Choose(int index)
    {
        Debug.Log("choose " + index);
        _engineInterface.Answer(index);
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
}
