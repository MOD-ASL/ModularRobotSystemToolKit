using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PartInteractionController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    private Renderer rend;

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
        moduleInteractionController.SetPartUnderMouse (null);
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
        moduleInteractionController = gameObject.GetComponentInParent<ModuleInteractionController> ();
    }

    // Highlight by changing the emission of the material
    public void HighlightOrNot (bool highlight) {
        if (highlight) {
            rend.material.SetColor ("_EmissionColor", Color.gray);
        }
        else {
            rend.material.SetColor ("_EmissionColor", Color.black);
        }
    }
}
