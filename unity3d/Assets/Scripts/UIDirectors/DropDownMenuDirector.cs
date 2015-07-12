using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DropDownMenuDirector : MonoBehaviour {

    public GameObject panelDropDown;
    public Animator dropDownMenuAnimator;

	// Use this for initialization
	void Start () {
	    HideMenu ();
	}
	
	// Update is called once per frame
	void Update () {

        // Hide the panel if the click is outside of it
        if (Input.GetMouseButtonDown (0))
        {
            if (!(EventSystem.current.IsPointerOverGameObject() &&
                EventSystem.current.currentSelectedGameObject &&
                (EventSystem.current.currentSelectedGameObject.transform.IsChildOf(transform)))) {
                HideMenu ();
            }
        }
	}

    // The button that triggers the drop down menu
    public void OnTriggerButtonClick () {
        ShowMenu ();
    }

    private void ShowMenu () {
        dropDownMenuAnimator.Play ("DropDownMenuFadeIn");
    }

    private void HideMenu () {
        dropDownMenuAnimator.Play ("DropDownMenuFadeOut");
    }
}
