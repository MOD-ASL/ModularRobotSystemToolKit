using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PartInteractionController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    private Renderer rend;

    #region IPointerEnterHandler implementation
    public void OnPointerEnter (PointerEventData eventData)
    {
        SendMessageUpwards ("OnMouseOverPartOrNot", true);
    }
    #endregion

    #region IPointerExitHandler implementation

    public void OnPointerExit (PointerEventData eventData)
    {
        SendMessageUpwards ("OnMouseOverPartOrNot", false);
    }

    #endregion

    #region IPointerClickHandler implementation

    public void OnPointerClick (PointerEventData eventData)
    {
        SendMessageUpwards ("OnMouseClick");
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
