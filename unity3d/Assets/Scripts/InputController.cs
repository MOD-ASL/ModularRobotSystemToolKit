using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void UpdateText (float newValue) {
		GetComponent<InputField>().text = newValue.ToString();
	}
}
