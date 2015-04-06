﻿using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	GameObject selectedModule;
	public GameObject panelModuleSetting;
	public GameObject panelSystem;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	// Setter of currentModule
	public void SetSelectedModule (GameObject module) {
		if (module == null) {
			selectedModule = module;
			panelModuleSetting.GetComponent<PanelModuleSettingController> ().Reset ();
			return;
		}
		if (selectedModule != module) {
			selectedModule = module;
			panelModuleSetting.GetComponent<PanelModuleSettingController> ().UpdateSelectedModule (selectedModule);
		}
	}

	public void ShowSystemPanel (bool show) {
		panelSystem.SetActive (show);
	}
}
