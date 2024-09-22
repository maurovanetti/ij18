using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public float startTime = 2.0f;
    public float waitTime = 10.0f;

    private Image curtain;
    private float lastActivityTime;
    private Vector2 pointerPosition;
    private bool isMonitoring;

    // Start is called before the first frame update
    void Start()
    {
        isMonitoring = false;
        curtain = GetComponent<Image>();
        curtain.enabled = (startTime > 0);
        Invoke(nameof(StartMonitoring), startTime);
    }

    void StartMonitoring()
    {
        isMonitoring = true;
        pointerPosition = Input.mousePosition;
        lastActivityTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMonitoring)
        {
            float now = Time.realtimeSinceStartup;
            Vector2 currentPointerPosition = Input.mousePosition;
            Vector2 delta = currentPointerPosition - pointerPosition;
            pointerPosition = currentPointerPosition;
            if (delta.magnitude > 0.1f || Input.anyKeyDown)
            {
                curtain.enabled = false;
                lastActivityTime = now;
            }
            else if (now - lastActivityTime > waitTime)
            {
                curtain.enabled = true;
            }
        }
    }
}
