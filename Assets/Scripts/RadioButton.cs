using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RadioButton : Clickable
{
    public Radio Radio;

    private void Update()
    {
        if (_activeButton)
            CheckInput();
    }

    public abstract void CheckInput();
}
