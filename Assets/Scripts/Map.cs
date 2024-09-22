using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour, IMap
{

    public GameObject topLeftCornerMarker;
    public GameObject bottomRightCornerMarker;

    private Vector2 topLeftCorner;
    private Vector2 bottomRightCorner;

    public GameObject[] Markers;
    public GameObject[] Trails;

    private void Start()
    {
        topLeftCorner = topLeftCornerMarker.transform.localPosition;
        bottomRightCorner = bottomRightCornerMarker.transform.localPosition;
        for (int i = 0; i < Markers.Length; i++)
        {
            Markers[i].SetActive(false);
        }
    }

    public int LastMarkerId()
    {
        return Markers.Length - 1;
    }

    public void UpdateMap(int channel)
    {
        if (channel >= 0 && channel < Markers.Length)
        {
            //check if channel has already been discovered
            if (!Markers[channel].activeInHierarchy)
            {
                DiscoverMarker(channel);
            }
            else
            {
                SetActiveMarker(channel);
            }
        }
    }

    public void DiscoverMarker(int channel)
    {
        Markers[channel].SetActive(true);
        SetActiveMarker(channel);
        MoveMarker(channel, Vector2.zero);
    }

    public void SetActiveMarker(int channel)
    {
        for (int i = 0; i < Markers.Length; i++)
        {
            GameObject marker = Markers[i];
            if (i == channel)
            {
                // ?
            }
            else
            {
                // ?
            }
        }        
    }

    public bool MoveMarker(int channel, Vector2 position)
    {
        Vector3 newPosition = ToMapCoordinates(position);
        Transform markerTransform = Markers[channel].transform;
        if ((markerTransform.localPosition - newPosition).magnitude < 0.0001f)
        {
            return false;
        }
        markerTransform.localPosition = newPosition;
        if (position == Vector2.zero)
        {
            markerTransform.Translate(Vector3.forward); // sink it
        }
        else
        {
            GameObject footstep = Trails[channel];
            if (footstep != null)
            {
                if (Vector2.Distance(footstep.transform.position, markerTransform.position) > 0.05f)
                {
                    GameObject newFootstep = GameObject.Instantiate(footstep, footstep.transform.position, Quaternion.identity); // make a copy of the previous footstep                    
                    footstep.transform.position = markerTransform.position; // move the footstep
                    footstep.SetActive(true);
                }
            }
        }
        // Debug.Log($"Marker #{channel} --> [{position.x}, {position.y}] = ({newPosition.x}, {newPosition.y})");
        return true;
    }

    private Vector2 ToMapCoordinates(Vector2 position01)
    {
        float left = topLeftCorner.x;
        float top = topLeftCorner.y;
        float bottom = bottomRightCorner.y;
        float right = bottomRightCorner.x;
        float dx = right - left;
        float dy = bottom - top;
        float x = left + position01.x * dx;
        float y = top + position01.y * dy;
        return new Vector2(x, y);
    }

    public Vector2 MarkerPosition(int channel)
    {
        if (!Markers[channel].activeInHierarchy)
        {
            return Vector2.zero;
        }
        Transform markerTransform = Markers[channel].transform;
        return FromMapCoordinates(markerTransform.localPosition);
    }

    private Vector2 FromMapCoordinates(Vector2 position)
    {
        float left = topLeftCorner.x;
        float top = topLeftCorner.y;
        float bottom = bottomRightCorner.y;
        float right = bottomRightCorner.x;
        float dx = right - left;
        float dy = bottom - top;
        float x01 = (position.x - left) / dx;
        float y01 = (position.y - top) / dy;
        return new Vector2(x01, y01);
    }
}
