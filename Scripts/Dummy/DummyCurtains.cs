using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCurtains : MonoBehaviour, ICurtains {

    public GameObject startGameCurtain;
    public GameObject gameOverCurtain;
    public GameObject victoryCurtain;

    private GameObject currentCurtain = null;

    public void ChangeTo(Curtain curtain)
    {
        if (currentCurtain != null)
        {
            currentCurtain.SetActive(false);
        }
        switch (curtain)
        {
            case Curtain.StartGame:
                currentCurtain = startGameCurtain;
                break;

            case Curtain.GameOver:
                currentCurtain = gameOverCurtain;
                break;

            case Curtain.Victory:
                currentCurtain = victoryCurtain;
                break;

            default:
            case Curtain.None:
                return;
        }
        currentCurtain.SetActive(true);
    }
}
