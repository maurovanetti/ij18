using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour, IRadio
{
    public Engine Engine;
    public int MaxNoiseValue;
    public NoiseControlButton[] NoiseControlButtons;
    public FrequencyButton[] FrequencyButtons;

    [Header("UI")]
    public Text FrequencyText;
    public Animator NoiseIndicator;

    private float _frequency;
    private int _noiseValue;
    private int _calculatedNoiseValue;
    private IEngine _engineInterface;

    // Use this for initialization
    void Start()
    {
        if (Engine)
            _engineInterface = Engine;
        StartCoroutine(InitRadio());
    }

    private IEnumerator InitRadio()
    {
        yield return new WaitForSeconds(1);
        UpdateFrequency();
    }

    public void UpdateFrequency()
    {
        _frequency = FrequencyButtons[0].ActualFrequency + (FrequencyButtons[1].ActualFrequency / 100);
        SetNoiseValue();
        FrequencyText.text = _frequency.ToString("N2");
        _engineInterface.TuneTo(_frequency,_noiseValue);
    }

    //set a new noise value (call when change frequency)
    public void SetNoiseValue()
    {
        _noiseValue = Random.Range(0, MaxNoiseValue);
        _calculatedNoiseValue = _noiseValue;
        ApplyNoiseControl();
    }

    //apply new noise control to noise value (call when change noise control)
    public void ApplyNoiseControl()
    {
        int noiseControlsValue = 0;
        foreach (NoiseControlButton button in NoiseControlButtons)
            noiseControlsValue += button.ActualValue;

        _calculatedNoiseValue = Mathf.Abs(_noiseValue - noiseControlsValue);

        NoiseIndicator.SetInteger("Value", _calculatedNoiseValue);
    }
}
