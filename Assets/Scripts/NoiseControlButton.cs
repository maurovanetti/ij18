using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseControlButton : RadioButton
{
    public int[] Values;
    public int InitialValue;
    public int ActualValue { get; set; }

    private Animator _animator;

    private void Start()
    {
        ActualValue = InitialValue;

        if (GetComponent<Animator>())
        {
            _animator = GetComponent<Animator>();
            _animator.SetInteger("Value", ActualValue);
        }
    }

    public override void CheckInput()
    {
        //if the button has more than two status
        if (Values.Length > 2)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
                UpdateValue(Input.GetAxis("Mouse ScrollWheel"));

            if (Input.GetMouseButtonDown(0))
                UpdateValue(1);

            if (Input.GetMouseButtonDown(1))
                UpdateValue(-1);
        }

        else
        {
            if (Input.GetMouseButtonDown(0))
                UpdateValue();
        }
    }

    public void UpdateValue()
    {
        ActualValue = ActualValue == Values[0] ? ActualValue = Values[1] : Values[0];
        if (_animator)
            _animator.SetInteger("Value", ActualValue);
        Radio.ApplyNoiseControl();
    }

    public void UpdateValue(float value)
    {
        if (value > 0)
            ActualValue++;
        if (value < 0)
            ActualValue--;

        if (ActualValue > Values[Values.Length - 1])
            ActualValue = Values[0];

        if (ActualValue < Values[0])
            ActualValue = Values[Values.Length - 1];

        if (_animator)
            _animator.SetInteger("Value", ActualValue);
        Radio.ApplyNoiseControl();
    }
}
