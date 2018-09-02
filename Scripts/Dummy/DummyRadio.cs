using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyRadio : MonoBehaviour, IRadio {

    public Engine engine;
    public InputField frequency;
    public Slider noise;

    private IEngine engineInterface;

	// Use this for initialization
	void Start () {
        engineInterface = engine;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Tune()
    {
        engineInterface.TuneTo(float.Parse(frequency.text), Mathf.RoundToInt(noise.value));
    }
}
