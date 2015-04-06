using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelSystemController : MonoBehaviour {
	public Text confName;
	public Button buttonSave;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (confName.text == "") {
			buttonSave.GetComponent<Button> ().interactable = false;
		}
		else {
			buttonSave.GetComponent<Button> ().interactable = true;
		}
	}
}
