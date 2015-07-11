using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

[Serializable]
public class AdditionalCallback : UnityEvent {}

public class ButtonDirector : MonoBehaviour {

    public bool createMode = false;
    public MenuTopDirector menuTopDirector;
    public string modeName = "";

    public AdditionalCallback additionalCallback;

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

    public void SetButtonInteractableOrNot (bool interactable) {
        // Do not disable button if it is in mode
        if (createMode && mode.status && !interactable) return;

        button.interactable = interactable;
    }
}
