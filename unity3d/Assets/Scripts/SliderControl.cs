using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Slider slider = gameObject.GetComponent<Slider> ();
		Debug.Log (slider.onValueChanged.GetPersistentTarget (1).name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
