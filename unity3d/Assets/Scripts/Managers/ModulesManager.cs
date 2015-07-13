using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulesManager : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;

    public Transform modulePrefab;
    public Transform robot;

    // Some config values
    public string moduleRootName;
    public Vector3 initialPosition;

    private GameObject anchorModule;
    private GameObject sensorModule;

	// Use this for initialization
	void Start () {
        Clear ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clear () {

        // Clear everything
        RemoveAllModules ();
        ma2MaComManager.selectionManager.ResetSelectedModule ();

        // Spawn a new one
        Transform clone = Instantiate (modulePrefab, initialPosition, Quaternion.identity) as Transform;
        clone.name = moduleRootName + "_0";
        clone.SetParent (robot.transform);

        SetAnchorModule (clone.gameObject);
    }

    void RemoveAllModules () {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in robot.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    public void DeleteSelectedModule () {
        if (ma2MaComManager.selectionManager.selectedModule) {
            DeleteModule (ma2MaComManager.selectionManager.selectedModule);
            ma2MaComManager.selectionManager.ResetSelectedModule ();
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.ResetTextMessage ();
        }
        else {
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Please select a module before click \"Delete\".");
        }
    }

    public void DeleteModule (GameObject module) {
        ma2MaComManager.selectionManager.selectedModule.GetComponent<ModuleConnectionController> ().DisconnectAllNodes ();
        Destroy (ma2MaComManager.selectionManager.selectedModule);
    }

    // When the Clear button is clicked
    public void OnClickClear () {
        Clear ();
    }

    // When the Delete button is clicked
    public void OnClickDelete () {
        DeleteSelectedModule ();
    }

    public Transform InsertModuleAt (Vector3 position, Quaternion rotation, GameObject connectedNode) {
        Transform clone = Instantiate (modulePrefab, position, rotation) as Transform;
        clone.name = FindNextAvailableName ();
        clone.parent = robot.transform;
        return clone;
    }

    string FindNextAvailableName () {
        int i = 1;
        while (robot.Find (moduleRootName + " " + i.ToString ()) != null) {
            i++;
        }
        return moduleRootName + " " + i.ToString ();
    }

    public void ShowConnectionsOrNot (bool show) {
        foreach (Transform m in robot) {
            m.GetComponent<ModuleConnectionController> ().ShowConnectionsOrNot (show);
        }
    }

    public void SetSelectedModuleAsAnchor () {
        if (ma2MaComManager.selectionManager.selectedModule) {
            SetAnchorModule (ma2MaComManager.selectionManager.selectedModule);
        }
        else {
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Please select a module before click \"Set Anchor\".");
        }
    }

    public void SetSelectedModuleAsSensor () {
        if (ma2MaComManager.selectionManager.selectedModule) {
            SetSensorModule (ma2MaComManager.selectionManager.selectedModule);
        }
        else {
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Please select a module before click \"Set Sensor\".");
        }
    }

    public void SetAnchorModule (GameObject module) {
        if (anchorModule != null) {
            anchorModule.GetComponent<ModuleModeController> ().SetAnchorOrNot (false);
        }
        anchorModule = module;
        anchorModule.GetComponent<ModuleModeController> ().SetAnchorOrNot (true);
    }

    public void SetSensorModule (GameObject module) {
        sensorModule = module;
    }

    public List<ModuleStateObject> GetAllModuleStateObjects () {
        List<ModuleStateObject> listOfModuleStateObjects = new List<ModuleStateObject> ();
        foreach (Transform m in robot) {
            listOfModuleStateObjects.Add (m.GetComponent<ModuleMotionController> ().GetModuleStateObject ());
        }
        return listOfModuleStateObjects;
    }

    public void SetAllModuleStateObjects (List<ModuleStateObject> listOfModuleStateObjects) {
        foreach (ModuleStateObject mso in listOfModuleStateObjects) {
            GameObject m = FindModuleWithName (mso.name);
            if (m != null) {
                m.GetComponent<ModuleMotionController> ().SetModuleStateObject (mso);
            }
            else {
                Debug.Log ("Cannot find module with name " + mso.name);
            }
        }
    }

    public GameObject FindModuleWithName (string name) {
        foreach (Transform m in robot) {
            if (m.name == name) {
                return m.gameObject;
            }
        }
        return null;
    }

}
