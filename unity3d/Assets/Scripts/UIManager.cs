using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	GameObject currentModule;
	GameObject canvasModuleSetting;

	// Use this for initialization
	void Start () {
		canvasModuleSetting = GameObject.Find ("CanvasModuleSetting");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.A)) {
			canvasModuleSetting.GetComponent<CanvasModuleSettingController> ().Show(true);
		}
		if (Input.GetKey (KeyCode.B)) {
			canvasModuleSetting.SetActive (false);
		}
	}

	// Setter of currentModule
	public void SetCurrentModule (GameObject module) {
		if (currentModule != module) {
			currentModule = module;
			canvasModuleSetting.GetComponent<CanvasModuleSettingController> ().Show(false);
		}
	}
}
