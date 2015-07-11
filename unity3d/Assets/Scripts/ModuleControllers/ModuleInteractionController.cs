using UnityEngine;
using System.Collections;

public class ModuleInteractionController : MonoBehaviour {

    [HideInInspector]
    public Mo2MaComController mo2MaComController;
    [HideInInspector]
    public GameObject partUnderMouse;

    private GameObject dummyModule;

    public GameObject dummyModulePrefab;

    private bool selected = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown (KeyCode.R)) {
            OnPressRKey ();
        }
	}

    void Awake () {
        mo2MaComController = gameObject.GetComponent<Mo2MaComController> ();
    }

    public void HighLightOrNot (bool highlight) {
        // If the module is selected then keep highligh unless deselected
        if (!(selected && !highlight)) {
            foreach (GameObject part in mo2MaComController.moduleRefPointerController.GetAllPartPointers ()) {
                part.GetComponent<PartInteractionController> ().HighlightOrNot (highlight);
            }
        }
    }

    public void SelectedOrNot (bool s) {
        selected = s;
        HighLightOrNot (selected);
    }

    public void OnMouseOverPartOrNot (bool mouseOverPart) {
        Mode addMode = mo2MaComController.ma2MoComManager.ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("Add");
        
        if (!addMode.status) {
            // Not in add mode
            HighLightOrNot (mouseOverPart);
        }
        else if (mouseOverPart && IsNodeAvailable (partUnderMouse)) {
            // In add mode and mouse is over a part
            dummyModule = Instantiate (dummyModulePrefab, GetDummyModulePosition (partUnderMouse), partUnderMouse.transform.rotation) as GameObject;
            dummyModule.tag = "Ghost";
            dummyModule.transform.Rotate (Vector3.right, 90.0f);
        }
        else if (!mouseOverPart) {
            // In add mode but mouse is off the module
            Destroy (dummyModule);
        }
    }

    private void OnPressRKey () {
        if (dummyModule != null) {
            dummyModule.transform.Rotate (dummyModule.transform.up, 90.0f, Space.World);
        }
    }

    public void OnMouseClick () {
        Mode addMode = mo2MaComController.ma2MoComManager.ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("Add");
        if (!addMode.status) {
            mo2MaComController.OnMouseClick ();
        }
        else {
            // In add mode and click on module
            GameObject partUnderMouseCache = partUnderMouse;
            Transform newModule = mo2MaComController.ma2MoComManager.ma2MaComManager.modulesManager.InsertModuleAt (dummyModule.transform.position,
                                                                                              dummyModule.transform.rotation,
                                                                                              partUnderMouse);;
            mo2MaComController.ma2MoComManager.ma2MaComManager.connectionManager.ConnectModule2Node (newModule.gameObject, partUnderMouseCache);
        }

    }

    public void SetPartUnderMouse (GameObject part) {
        partUnderMouse = part;
    }

    public void ResetPartUnderMouse () {
        SetPartUnderMouse (null);
    }

    private Vector3 GetDummyModulePosition (GameObject node) {
        if (node.name == "FrontWheel") {
            return node.transform.position + node.transform.right;
        }
        else if (node.name == "BackPlate") {
            return node.transform.position - node.transform.right;
        }
        else if (node.name == "LeftWheel") {
            return node.transform.position - node.transform.up;
        }
        else {
            return node.transform.position + node.transform.up;
        }
    }

    private bool IsNodeAvailable (GameObject node) {
        return mo2MaComController.moduleConnectionController.IsNodeAvailable (node);
    }
}
