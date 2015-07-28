﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PartInteractionController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    private Renderer rend;
    private Color originalColor;

    [HideInInspector]
    public bool selected = false;

    private ModuleInteractionController moduleInteractionController;

    #region IPointerEnterHandler implementation
    public void OnPointerEnter (PointerEventData eventData)
    {
        moduleInteractionController.SetPartUnderMouse (gameObject);
        moduleInteractionController.OnMouseOverPartOrNot (true);
    }
    #endregion

    #region IPointerExitHandler implementation

    public void OnPointerExit (PointerEventData eventData)
    {
        moduleInteractionController.ResetPartUnderMouse ();
        moduleInteractionController.OnMouseOverPartOrNot (false);
    }

    #endregion

    #region IPointerClickHandler implementation

    public void OnPointerClick (PointerEventData eventData)
    {
        moduleInteractionController.OnMouseClick ();
    }

    #endregion

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake (){
        rend = gameObject.GetComponent<Renderer> ();
        rend.material.EnableKeyword ("_EMISSION");
        originalColor = rend.material.color;
        moduleInteractionController = gameObject.GetComponentInParent<ModuleInteractionController> ();
    }

    // Highlight by changing the emission of the material
    public void HighlightOrNot (bool highlight) {
        if (highlight || selected) {
			rend.material.color = Color.white;
        }
        else {
			rend.material.color = originalColor;
        }
    }

    public void ShowConnectionsOrNot (bool show, bool connected) {
        if (show) {
            if (connected) {
                rend.material.color = Color.red;
            }
            else {
                rend.material.color = Color.green;
            }
        }
        else {
            rend.material.color = originalColor;
        }
    }

    public void SelectOrNot (bool select) {
        selected = select;
        HighlightOrNot (select);
    }
}
