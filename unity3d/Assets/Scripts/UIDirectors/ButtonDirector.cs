using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AdditionalCallback : UnityEvent {}

public class ButtonDirector : MonoBehaviour {

    public bool createMode = false;
    public MenuTopDirector menuTopDirector;
    public string modeName = "";

    public AdditionalCallback additionalCallback;

    public List<ButtonDirector> enableInModeFromButtons;
    public List<ButtonDirector> disableNotInModeFromButtons;

    [HideInInspector]
    public Mode mode;
    private Button button;
    private Text buttonText;

	// Use this for initialization
	void Start () {

        button = gameObject.GetComponent<Button> ();
        buttonText = gameObject.GetComponentInChildren<Text> ();

        if (createMode) {
            mode = menuTopDirector.uI2MaComDirector.ma2UIComManager.ma2MaComManager.modeManagerNew.GetOrCreateModeByName ((modeName == "") ? buttonText.text : modeName);
            button.onClick.AddListener (() => {UpdateColorFromMode ();});
            button.onClick.AddListener (() => {menuTopDirector.OnUpdateMode ();});
            button.onClick.AddListener (additionalCallback.Invoke);
        }

        SetButtonInteractableOrNot (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateColorFromMode () {
        mode.ToggleMode ();

        if (mode.status) {
            button.image.color = button.colors.pressedColor;
        }
        else {
            ResetColor ();
        }

        // Change interactability of other buttons
        menuTopDirector.SetAllButtonInteractableOrNot (!mode.status);
    }

    public void ResetColor () {
        button.image.color = button.colors.highlightedColor;
    }

    // Check if there is any mode in the enable list is ON
    private bool CheckOtherModesON () {
        if (enableInModeFromButtons != null) {
            foreach (ButtonDirector bd in enableInModeFromButtons) {
                if (bd.mode.status) {
                    return true;
                }
            }
        }
        return false;
    }

    // Check if there is any mode in the disable list is OFF
    private bool CheckOtherModesOFF () {
        if (disableNotInModeFromButtons != null) {
            foreach (ButtonDirector bd in disableNotInModeFromButtons) {
                if (!bd.mode.status) {
                    return true;
                }
            }
        }
        return false;
    }


    public void SetButtonInteractableOrNot (bool interactable) {
        button.interactable = interactable;

        // Do not disable button if it is in mode or is effected by other modes
        if (createMode && mode.status && !interactable) button.interactable = true;

        if (CheckOtherModesON ()) button.interactable = true;

        if (CheckOtherModesOFF ()) button.interactable = false;

    }
}
