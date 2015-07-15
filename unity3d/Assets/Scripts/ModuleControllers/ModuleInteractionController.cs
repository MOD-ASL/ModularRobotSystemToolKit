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
        foreach (GameObject part in mo2MaComController.moduleRefPointerController.GetAllPartPointers ()) {
            part.GetComponent<PartInteractionController> ().SelectOrNot (s);
        }
        HighLightOrNot (selected);
    }

    public void OnMouseOverPartOrNot (bool mouseOverPart) {
        Mode addMode = mo2MaComController.ma2MoComManager.ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("Add");
        Mode disOrConnectMode = mo2MaComController.ma2MoComManager.ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("DisOrConnect");
        
        if (addMode.status) {
            if (mouseOverPart && IsNodeAvailable (partUnderMouse)) {
                // In add mode and mouse is over a part
                dummyModule = Instantiate (dummyModulePrefab, GetDummyModulePosition (partUnderMouse), partUnderMouse.transform.rotation) as GameObject;
                dummyModule.tag = "Ghost";
                dummyModule.transform.Rotate (Vector3.right, 90.0f);
            }
            else if (!mouseOverPart) {
                // In add mode but mouse is off the module
                Destroy (dummyModule);
                mo2MaComController.ma2MoComManager.ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.ResetTextMessage ();
            }
        }
        else if (disOrConnectMode.status) {
            // In disconnect or connect mode
            mo2MaComController.ma2MoComManager.ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.ResetTextMessage ();
            if ((partUnderMouse != null) && (partUnderMouse.tag == "Node")) {
                partUnderMouse.GetComponent<PartInteractionController> ().HighlightOrNot (mouseOverPart);
                GameObject touchedNode = partUnderMouse.GetComponent<PartController> ().touchedNode;
                if (partUnderMouse.transform.parent.parent.name == "newRobot" || mo2MaComController.ma2MoComManager.ma2MaComManager.robotManager.nodeOnNewRobot != null) {

                }
                else if (touchedNode != null) {
                    touchedNode.GetComponent<PartInteractionController> ().HighlightOrNot (mouseOverPart);
                }
                else {
                    mo2MaComController.ma2MoComManager.ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot connect modules: No adjacent module found.");
                }
            }
        }
        else {
            HighLightOrNot (mouseOverPart);
        }
    }

    private void OnPressRKey () {
        if (dummyModule != null) {
            dummyModule.transform.Rotate (dummyModule.transform.up, 90.0f, Space.World);
        }
    }

    public void OnMouseClick () {
        Mode addMode = mo2MaComController.ma2MoComManager.ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("Add");
        Mode disOrConnectMode = mo2MaComController.ma2MoComManager.ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("DisOrConnect");

        if (addMode.status) {
            // In add mode and click on module
            // Check if there is collision
            if (!dummyModule.GetComponent<DummyModuleController> ().collision) {
                GameObject partUnderMouseCache = partUnderMouse;
                Transform newModule = mo2MaComController.ma2MoComManager.ma2MaComManager.modulesManager.InsertModuleAt (dummyModule.transform.position,
                                                                                                                        dummyModule.transform.rotation
                                                                                                                        );
                newModule.GetComponent<ModuleModeController> ().SetTrigger (false);
                StartCoroutine (mo2MaComController.ma2MoComManager.ma2MaComManager.connectionManager.ConnectModule2Node (newModule.gameObject, partUnderMouseCache));
            }
            else {
                mo2MaComController.ma2MoComManager.ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot add new module: Collision detected.");
            }
        }
        else if (disOrConnectMode.status) {
            if (partUnderMouse.tag == "Node") {
                // When click left mouse button in DisOrConnect mode
                GameObject touchedNode = partUnderMouse.GetComponent<PartController> ().touchedNode;
                if (partUnderMouse.transform.parent.parent.name == "newRobot") {
                    mo2MaComController.ma2MoComManager.ma2MaComManager.robotManager.nodeOnNewRobot = partUnderMouse;
                }
                else if (mo2MaComController.ma2MoComManager.ma2MaComManager.robotManager.nodeOnNewRobot != null) {
                    if (partUnderMouse.transform.parent.GetComponent<ModuleConnectionController> ().IsNodeAvailable (partUnderMouse)) {
                        mo2MaComController.ma2MoComManager.ma2MaComManager.robotManager.MoveNewRobot (mo2MaComController.ma2MoComManager.ma2MaComManager.robotManager.nodeOnNewRobot, partUnderMouse);
                        mo2MaComController.ma2MoComManager.ma2MaComManager.robotManager.nodeOnNewRobot = null;
                    }
                }
                else if (touchedNode != null) {
                    // Toggle the connection and display the new color
                    mo2MaComController.ma2MoComManager.ma2MaComManager.connectionManager.ToggleConnection (partUnderMouse, touchedNode);
                    mo2MaComController.moduleConnectionController.ShowConnectionsOrNot (true);
                    touchedNode.transform.parent.GetComponent <ModuleConnectionController> ().ShowConnectionsOrNot (true);
                }
            }
        }
        else {
            mo2MaComController.OnMouseClick ();
        }

    }

    public void SetPartUnderMouse (GameObject part) {
        partUnderMouse = part;
    }

    public void ResetPartUnderMouse () {
        partUnderMouse.GetComponent<PartInteractionController> ().HighlightOrNot (false);
        GameObject touchedNode = partUnderMouse.GetComponent<PartController> ().touchedNode;
        if (touchedNode != null) {
            touchedNode.GetComponent<PartInteractionController> ().HighlightOrNot (false);
        }
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
