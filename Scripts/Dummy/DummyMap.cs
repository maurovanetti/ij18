using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMap : MonoBehaviour, IMap {

    public void MoveMarker(int channel, Vector2 position)
    {
        Debug.Log(channel + ": MARKER @ " + position);
    }

    public void UpdateMap(int channel)
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
