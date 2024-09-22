using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCurtains : MonoBehaviour/*, ICurtains*/ {

    public GameObject startGameCurtain;
    public GameObject gameOverCurtain;
    public GameObject victoryCurtain;

    public GameObject radio;

    // private GameObject currentCurtain = null;

    //public void ChangeTo(Curtain curtain)
    //{
    //    if (currentCurtain != null)
    //    {
    //        currentCurtain.SetActive(false);
    //    }
    //    switch (curtain)
    //    {
    //        case Curtain.StartGame:                
    //            currentCurtain = startGameCurtain;
    //            break;

    //        case Curtain.GameOver:                
    //            currentCurtain = gameOverCurtain;
    //            break;

    //        case Curtain.Victory:                
    //            currentCurtain = victoryCurtain;
    //            break;

    //        default:
    //        case Curtain.None:
    //            radio.SetActive(true);
    //            return;
    //    }
    //    radio.SetActive(false);
    //    currentCurtain.SetActive(true);
    ////}
}
