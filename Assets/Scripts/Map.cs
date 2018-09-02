using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour, IMap
{
    public GameObject[] Markers;

    private void Start()
    {
        for (int i = 0; i < Markers.Length; i++)
            Markers[i].SetActive(false);
    }

    public void UpdateMap(int channel)
    {
        //check if channel has already been discovered
        if (!Markers[channel].activeInHierarchy)
            DiscoverMarker(channel);
        else
            SetActiveMarker(channel);
    }

    public void DiscoverMarker(int channel)
    {
        Markers[channel].SetActive(true);
        SetActiveMarker(channel);
    }

    public void SetActiveMarker(int channel)
    {
        for (int i = 0; i < Markers.Length; i++)
            Markers[i].GetComponent<Animator>().SetBool("Active", false);

        Markers[channel].GetComponent<Animator>().SetBool("Active", true);
    }

    public void MoveMarker(int channel, Vector2 position)
    {
        Markers[channel].transform.Translate(position);
    }
}
