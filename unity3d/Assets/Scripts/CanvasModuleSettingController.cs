using UnityEngine;
using System.Collections;

public class CanvasModuleSettingController : MonoBehaviour {

	GameObject currentModule;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show (bool showCanvas) {
		gameObject.SetActive (showCanvas);
	}

	// Setter of currentModule
	public void SetCurrentModule (GameObject module) {
		currentModule = module;
	}
}
