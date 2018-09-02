using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RadioButton : MonoBehaviour
{
    public Radio Radio;
    public GameObject Halo;
    private bool _activeButton;

    private void Update()
    {
        if (_activeButton)
            CheckInput();
    }

    public void OnOverEnter()
    {
        _activeButton = true;
        Highlight();
    }


    public void OnOverExit()
    {
        _activeButton = false;
        Darken();
    }

    public void Highlight()
    {
        if (Halo)
            Halo.SetActive(true);
    }

    public void Darken()
    {
        if (Halo)
            Halo.SetActive(false);
    }

    public abstract void CheckInput();
}
