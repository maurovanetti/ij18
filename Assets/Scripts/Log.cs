using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Log : MonoBehaviour, ILog
{
    public Engine engine;
    public GameObject TextBlockPrefab;
    public GameObject TextSeparatorPrefab;
    public GameObject TextAnswerPrefab;
    public Color PlayerTextColor;
    public Color NPCTextColor;
    public GameObject WhiteNoise;
    public GameObject GreenLight;
    public GameObject RedLight;
    public GameObject[] TeamChannelLight;
    public GameObject[] ChannelIconLight; //static icon: if on channel -> on else -> off 
    public GameObject[] ChannelScrollViewContent;
    public GameObject AnswerBoard;
    public GameObject[] AnswersScrollViewContent;

    [Header("Radio Screen")]
    public SpriteRenderer RadioScreen;
    public Color RadioScreenDefaultColor;
    public Color[] RadioScreenChannelColors;

    [Header("Noise")]
    public float NoiseToughness;
    public float NoiseFinalToughness;
    public int MessagesRequiredToEaseNoise;

    private IEngine _engineInterface;
    private int _activeChannelIndex;
    private int _noise;
    private ISet<int> _talkingChannels = new HashSet<int>();
    private List<GameObject> _recentTextBlocks = new List<GameObject>();
    private Dictionary<Text, string> _exactTexts = new Dictionary<Text, string>();
    private GameObject _activeTextBlock;
    private string[] _symbols = new string[] { "¤", "¤", "-", "-", "-", "#", "#"};
    // private string[] _symbols = new string[] { "Ç", "Ø", "Þ", "©", "¤", "§", "°", "¾", "¥", "¿" };
    private RectTransform _activeScrollViewContent;
    private RectTransform _activeAnswerScrollViewContent;
    private float _noiseToughnessReductionPerMessage;

    // Use this for initialization
    void Start()
    {
        _engineInterface = engine;
        _noiseToughnessReductionPerMessage = (NoiseToughness - NoiseFinalToughness) / MessagesRequiredToEaseNoise;
    }

    void Update()
    {
        for (int i = 0; i < TeamChannelLight.Length; i++)
        {
            GameObject light = TeamChannelLight[i];
            if (_talkingChannels.Contains(i))
            {
                if (i == _activeChannelIndex && !WhiteNoise.activeSelf)
                {
                    light.SetActive(true);
                }
                else
                {
                    light.SetActive(Time.time % 1f > 0.5f);
                }                
            }
            else
            {
                light.SetActive(false);
            }
        }
    }

    public void NotifyTuned(int channel, string frequency, int noise)
    {        
        _activeChannelIndex = channel;
        _noise = noise;        
        _activeScrollViewContent = ChannelScrollViewContent[channel].GetComponent<RectTransform>();
        _activeAnswerScrollViewContent = AnswersScrollViewContent[channel].GetComponent<RectTransform>();
        SwitchStatus(true);
    }

    public void LoseConnection()
    {
        SwitchStatus(false);
    }

    private GameObject Grandparent(GameObject go)
    {
        return go.transform.parent.parent.gameObject;
    }

    private void Highlight(bool on)
    {
        ChannelScrollViewContent[_activeChannelIndex].transform.parent.parent.Find("Highlight").GetComponent<Image>().enabled = on;
    }

    private void SwitchStatus(bool connected)
    {
        WhiteNoise.SetActive(!connected);
        GreenLight.SetActive(connected);
        RedLight.SetActive(!connected);        
        ChannelIconLight[_activeChannelIndex].SetActive(connected);
        GameObject channelScrollViewContent = ChannelScrollViewContent[_activeChannelIndex];
        Grandparent(channelScrollViewContent).SetActive(connected);
        channelScrollViewContent.SetActive(connected);
        GameObject answerScrollViewContent = AnswersScrollViewContent[_activeChannelIndex];
        Grandparent(answerScrollViewContent).SetActive(connected);
        bool displayAnswerBoard = connected && _activeAnswerScrollViewContent.childCount > 0;
        AnswerBoard.GetComponent<Animator>().SetBool("Show", displayAnswerBoard);
        if (connected)
        {
            ChannelScrollViewContent[_activeChannelIndex].transform.SetAsLastSibling();
            AnswersScrollViewContent[_activeChannelIndex].transform.SetAsLastSibling();
            RadioScreen.color = RadioScreenChannelColors[_activeChannelIndex];            
        }
        else
        {
            _recentTextBlocks.Clear();
            _exactTexts.Clear();
            RadioScreen.color = RadioScreenDefaultColor;
        }
    }

    public void NotifyReady(int channel)
    {
        _talkingChannels.Add(channel);
    }

    private void SetSizeDelta(GameObject block, float deltaHeight)
    {
        Vector2 delta = Vector2.up * deltaHeight;
        block.GetComponent<RectTransform>().sizeDelta += delta;
        _activeScrollViewContent.sizeDelta += delta;
    }

    public void Break(int channel)
    {
        GameObject textSeparatorBlock = Instantiate(TextSeparatorPrefab, _activeScrollViewContent);
        _recentTextBlocks.Add(textSeparatorBlock);

        SetSizeDelta(textSeparatorBlock, 10f);
        Highlight(false);
        _talkingChannels.Remove(channel);
    }

    public void Send(int channel, string sentence)
    {
        bool isPlayerSentence = sentence.StartsWith("/");
        _talkingChannels.Add(channel);
        string textToShow = ItalicizeAvatarVoice(ApplyNoise(sentence));
        GameObject textBlock = Instantiate(TextBlockPrefab, _activeScrollViewContent);
        _activeTextBlock = textBlock;
        _recentTextBlocks.Add(_activeTextBlock);
        _exactTexts.Add(textBlock.GetComponentInChildren<Text>(), sentence);
        textBlock.GetComponentInChildren<Text>().text = textToShow;

        if (isPlayerSentence)
        {
            textBlock.GetComponentInChildren<Text>().color = PlayerTextColor;
        }            
        else
        {
            textBlock.GetComponentInChildren<Text>().color = NPCTextColor;
        }

        SetSizeDelta(textBlock, CalcBlockHeight(sentence, 40f, 50, 2));

        if (NoiseToughness > NoiseFinalToughness)
        {
            NoiseToughness -= _noiseToughnessReductionPerMessage;
        }
        Highlight(true);
    }

    private string ApplyNoise(string sentence)
    {
        if (sentence.StartsWith("/"))
        {
            return sentence;
        }
        // Remove stuff in brackets
        char[] sentenceChars = sentence.ToCharArray();

        List<int> indecesToEncrypt = new List<int>();
        List<int> indecesToSkip = new List<int>();
        bool keepLikeThis = false;
        for (int k = 0; k < sentenceChars.Length; k++)
        {
            if (sentenceChars[k] == '(')
            {
                keepLikeThis = true;                
            }
            if (keepLikeThis || !char.IsLetterOrDigit(sentenceChars[k]))
            {
                indecesToSkip.Add(k);
                if (sentenceChars[k] == ')')
                {
                    keepLikeThis = false;
                }
            }
            else
            {
                indecesToEncrypt.Add(k);
            }
        }

        float noise = _noise * NoiseToughness;
        float floatCharToCrypt = (indecesToEncrypt.Count / 100f) * noise;
        int charToCrypt = Mathf.CeilToInt(floatCharToCrypt);        
        int i = 0;
        while (i < charToCrypt && indecesToEncrypt.Count > 0)
        {
            int charIndexIndex = UnityEngine.Random.Range(0, indecesToEncrypt.Count);
            int charIndex = indecesToEncrypt[charIndexIndex];
            sentenceChars[charIndex] = Convert.ToChar(_symbols[UnityEngine.Random.Range(0, _symbols.Length)]);
            indecesToEncrypt.Remove(charIndex);
            i++;
        }
        return new string(sentenceChars);
    }

    private float CalcBlockHeight(string sentence, float heightPerLine, int charPerLine, int minLines)
    {        
        int lines = 1 + (sentence.Length / charPerLine);
        if (lines < minLines)
        {
            lines = minLines;
        }        
        return heightPerLine * lines;
    }

    public string ItalicizeAvatarVoice(string original)
    {
        if (original.StartsWith("/"))
        {
            return string.Format("<i>{0}</i>", original.Substring(1));
        }
        else
        {
            return original;
        }
    }

    public void Ask(int channel, string[] options)
    {
        float initialSpacing = 5f;
        _talkingChannels.Add(channel);
        for (int i = 0; i < options.Length; i++)
        {
            AnswerBlock answerBlock = Instantiate(TextAnswerPrefab, _activeAnswerScrollViewContent).GetComponent<AnswerBlock>();
            VerticalLayoutGroup answerBlockLayout = answerBlock.GetComponent<VerticalLayoutGroup>();
            float spacing = (answerBlockLayout.padding.top + answerBlockLayout.padding.bottom) /2;
            answerBlock.SetPosition(_activeAnswerScrollViewContent.sizeDelta.y + initialSpacing);
            answerBlock.GetComponentInChildren<Text>().text = /* (i + 1) + ". " + */ ItalicizeAvatarVoice(options[i]);
            Canvas.ForceUpdateCanvases();
            //answerBlock.GetComponent<RectTransform>().sizeDelta += new Vector2(0, CalcBlockHeight(options[i], 50f, 25, 1));
            _activeAnswerScrollViewContent.sizeDelta += new Vector2(0, answerBlock.Height + spacing);
            int index = i;
            answerBlock.GetComponent<Button>().onClick.AddListener(() => { Choose(index); });
        }

        AnswerBoard.GetComponent<Animator>().SetBool("Show", true);
        Highlight(true);
    }

    private void Choose(int index)
    {     
        _engineInterface.Answer(index);
        foreach (Transform child in _activeAnswerScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        _activeAnswerScrollViewContent.sizeDelta = new Vector2(_activeAnswerScrollViewContent.sizeDelta.x, 0);
        AnswerBoard.GetComponent<Animator>().SetBool("Show", false);
    }

    public void UpdateNoise()
    {        
        foreach (GameObject recentTextBlock in _recentTextBlocks)
        {
            Text textToChange = recentTextBlock.GetComponentInChildren<Text>();
            if (textToChange)
            {
                string exactText = _exactTexts[textToChange];
                textToChange.text = ItalicizeAvatarVoice(ApplyNoise(exactText));
            }
        }
    }
}
