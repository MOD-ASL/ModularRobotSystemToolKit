using UnityEngine;
using System.Collections;

public class ModuleInteractionController : MonoBehaviour {

    public Mo2MaComController mo2MaComController;
    public ModuleRefPointerController moduleRefPointerController;

    private bool selected = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HighLightOrNot (bool highlight) {
        // If the module is selected then keep highligh unless deselected
        if (!(selected && !highlight)) {
            foreach (GameObject part in moduleRefPointerController.GetAllPartPointers ()) {
                part.GetComponent<PartInteractionController> ().HighlightOrNot (highlight);
            }
        }
    }

    public void SelectedOrNot (bool s) {
        selected = s;
        HighLightOrNot (selected);
    }

    void OnMouseOverPartOrNot (bool mouseOverPart) {
        HighLightOrNot (mouseOverPart);
    }

    void OnMouseClick () {
        mo2MaComController.OnMouseClick ();
    }
}
