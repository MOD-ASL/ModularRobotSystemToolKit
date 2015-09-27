using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderTextControl : MonoBehaviour {
	public Text sliderValue;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void UpdateText (float newValue) {
		sliderValue.text = newValue.ToString() + " Degree";
	}
}
