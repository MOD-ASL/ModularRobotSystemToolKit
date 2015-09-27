using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelTopMenuController : MonoBehaviour {
	
	public Button buttonSimulate;
	public Button buttonClear;
	public Button buttonBringToGround;
	public Button buttonModuleSetting;

	private List<Button> allButtons;

	public ColorManager colorManager;
	public ModeManager modeManager;

	// Use this for initialization
	void Start () {
		allButtons = new List<Button> () {buttonSimulate, buttonClear, buttonModuleSetting, buttonBringToGround};
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Set all buttons to be interactable or not
	void SetAllButtonsInteractableOrNot (bool interactable) {
		allButtons.ForEach (b => b.GetComponent<Button> ().interactable = interactable);
	}

	public void ChangeMode () {
		UpdateButtonColor ();
	}

    void OnButtonInModeOrNot (Button b, bool inMode) {
        ColorBlock cb;
        cb = b.colors;
        if (inMode) {
            cb.normalColor =  colorManager.buttonInMode;
        }
        else {
            cb.normalColor =  colorManager.buttonNormal;
        }
        b.colors = cb;
    }

	void UpdateButtonColor () {
		SetAllButtonsInteractableOrNot (false);
		if (modeManager.IsSimulate) {
            OnButtonInModeOrNot (buttonSimulate, true);
			buttonSimulate.GetComponent<Button> ().interactable = true;
		}
		else if (modeManager.IsAddModule) {}
		else if (modeManager.IsConnectNodes) {}
		else if (modeManager.IsSystem) {}
		else if (modeManager.IsRecordBehavior) {
            buttonModuleSetting.GetComponent<Button> ().interactable = true;
        }
		else {
			OnButtonInModeOrNot (buttonSimulate, false);
			SetAllButtonsInteractableOrNot (true);
		}
	}
}
