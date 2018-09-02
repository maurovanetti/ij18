using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyButton : RadioButton
{
    public int[] Frequencies;
    public float InitialFrequency;
    public float ActualFrequency { get; set; }
    //how much turn the knob increase or decrease the frequency
    public float FrequencyChangeValue;

    public void Start()
    {
        ActualFrequency = InitialFrequency;
    }

    public override void CheckInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            UpdateFrequency(Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetMouseButtonDown(0))
            UpdateFrequency(1);

        if (Input.GetMouseButtonDown(1))
            UpdateFrequency(-1);
    }

    public void UpdateFrequency(float updateValue)
    {
        ActualFrequency = updateValue > 0 ? ActualFrequency + FrequencyChangeValue : ActualFrequency - FrequencyChangeValue;

        if (ActualFrequency > Frequencies[Frequencies.Length - 1])
            ActualFrequency = Frequencies[0];

        if (ActualFrequency < Frequencies[0])
            ActualFrequency = Frequencies[Frequencies.Length - 1];

        Radio.UpdateFrequency();
    }
}
