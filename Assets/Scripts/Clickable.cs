using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    protected bool _activeButton;
    public Texture2D cursor;
    
    //public void OnOverEnter()
    //{
    //    _activeButton = true;
    //    Highlight();
    //}

    //New method for scene Main2
    public virtual void OnMouseEnter()
    {
        _activeButton = true;
        Highlight();
    }

    //public void OnOverExit()
    //{
    //    _activeButton = false;
    //    Darken();
    //}

    //New method for scene Main2
    public virtual void OnMouseExit()
    {
        _activeButton = false;
        Darken();
    }

    public void Highlight()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    public void Darken()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
