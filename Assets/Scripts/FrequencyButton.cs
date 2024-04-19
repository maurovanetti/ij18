using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyButton : RadioButton
{
    public GameObject button;
    public int[] Frequencies;
    public float ActualFrequency
    {
        get
        {
            return Frequencies[_index];
        }

    }
    private int _index;
    public AudioClip ButtonSound;

    public void Start()
    {
        _index = 0;
    }

    public override void CheckInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            UpdateFrequency(Input.GetAxis("Mouse ScrollWheel") > 0 ? +1 : -1);

        if (Input.GetMouseButtonDown(0))
            UpdateFrequency(+1);

        if (Input.GetMouseButtonDown(1))
            UpdateFrequency(-1);
    }

    public void UpdateFrequency(int updateValue)
    {
        _index += updateValue;
        if (_index >= Frequencies.Length)
        {
            _index = 0;
        }
        else if (_index < 0)
        {
            _index = Frequencies.Length - 1;
        }        
        SoundManager.Instance.PlaySingleSound(ButtonSound);
        Radio.UpdateFrequency();
        button.transform.Rotate(Vector3.back, updateValue * 360.0f / Frequencies.Length);
    }
}
