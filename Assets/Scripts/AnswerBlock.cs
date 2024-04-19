using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerBlock : Clickable
{
    public float Height { get { return GetComponent<RectTransform>().sizeDelta.y; } }
    public Sprite DefaultBackground;
    public Sprite OnOverBackground;
    public Color DefaultTextColor;
    public Color HighlightTextColor;
    public Text Text;
    public Image Frame;

    public override void OnMouseEnter()
    {
        OnOverEnter();
    }

    public override void OnMouseExit()
    {
        OnOverExit();
    }

    public void OnOverEnter()
    {
        Frame.sprite = OnOverBackground;
        Text.color = HighlightTextColor;
        Highlight();
    }


    public void OnOverExit()
    {
        Frame.sprite = DefaultBackground;
        Text.color = DefaultTextColor;
        Darken();
    }

    public void SetPosition(float height)
    {
        RectTransform container = GetComponent<RectTransform>();
        container.anchoredPosition = new Vector2(container.anchoredPosition.x, -height);
    }
}
