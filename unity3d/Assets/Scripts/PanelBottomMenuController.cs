using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelBottomMenuController : MonoBehaviour {

	public Button buttonSystem;
	public Button buttonAddModule;
	public Button buttonDeleteModule;
	public Button buttonConnectNodes;
	public Button buttonConnect;
    public Button buttonRecordBehavior;

    private List<Button> allButtons;

	ColorManager colorManager;
	UIManager UIManagerScript;
    public ModeManager modeManager;

	// Use this for initialization
	void Start () {
		UIManagerScript = (UIManager) GameObject.FindObjectOfType<UIManager> ();
		colorManager = (ColorManager) GameObject.FindObjectOfType<ColorManager> ();
        allButtons = new List<Button> () {buttonSystem, buttonAddModule, buttonDeleteModule, buttonConnect, buttonConnectNodes, buttonRecordBehavior};
	}

    // Set all buttons to be interactable or not
    void SetAllButtonsInteractableOrNot (bool interactable) {
        allButtons.ForEach (b => b.GetComponent<Button> ().interactable = interactable);
    }

    public void ChangeMode () {
        UpdateButtonColor ();
    }

	void UpdateButtonColor () {
        SetAllButtonsInteractableOrNot (false);
		if (modeManager.IsAddModule) {
            OnButtonInModeOrNot (buttonAddModule, true);
			buttonAddModule.GetComponent<Button> ().interactable = true;
		}
		else if (modeManager.IsConnectNodes) {
            OnButtonInModeOrNot (buttonConnectNodes, true);
            buttonConnectNodes.GetComponent<Button> ().interactable = true;

            OnConnectNodes ();
		}
		else if (modeManager.IsSimulate) {}
        else if (modeManager.IsRecordBehavior) {}
		else if (modeManager.IsSystem) {
            OnButtonInModeOrNot (buttonSystem, true);
            buttonSystem.GetComponent<Button> ().interactable = true;
            UIManagerScript.ShowSystemPanel (true);
		}
		else {
            OnButtonInModeOrNot (buttonAddModule, false);
            OnButtonInModeOrNot (buttonConnectNodes, false);
            OnButtonInModeOrNot (buttonSystem, false);
            SetAllButtonsInteractableOrNot (true);
            UIManagerScript.ShowSystemPanel (modeManager.IsSystem);
            OnConnectNodes ();
		}
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

    void OnConnectNodes () {
        buttonConnect.gameObject.SetActive (modeManager.IsConnectNodes);
        Vector2 size = gameObject.GetComponent<RectTransform> ().sizeDelta;
        size.y = (modeManager.IsConnectNodes) ? 96.0f : 48.0f;
        gameObject.GetComponent<RectTransform> ().sizeDelta = size;
    }
    
    // Update is called once per frame
	void Update () {
	
	}
}
