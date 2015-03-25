using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelBottomMenuController : MonoBehaviour {
	bool isSimulate = false;
	bool isAddModule = false;
	public Button buttonAddModule;
	ColorManager colorManager;

	// Use this for initialization
	void Start () {
		colorManager = (ColorManager) GameObject.FindObjectOfType<ColorManager> ();
	}

	public void OnClickSimulate (float data) {
		isSimulate = !isSimulate;
		UpdateButtonColor ();
	}

	public void OnClickAddModule (float data) {
		isAddModule = !isAddModule;
		UpdateButtonColor ();
	}

	void UpdateButtonColor () {
		ColorBlock cb;
		if (isAddModule) {
			cb = buttonAddModule.colors;
			cb.normalColor =  colorManager.buttonInMode;
			buttonAddModule.colors = cb;
		}
		else if (isSimulate) {
			buttonAddModule.GetComponent<Button> ().interactable = false;
		}
		else {
			cb = buttonAddModule.colors;
			cb.normalColor =  colorManager.buttonNormal;
			buttonAddModule.colors = cb;
			buttonAddModule.GetComponent<Button> ().interactable = true;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
