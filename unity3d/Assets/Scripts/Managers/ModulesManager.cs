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

    public GameObject anchorModule;
    private GameObject sensorModule;

    private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
        Clear ();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Clear (bool spawn = true) {

        // Clear everything
        RemoveAllModules ();
        ma2MaComManager.selectionManager.ResetSelectedModule ();
		ma2MaComManager.modulesManager.anchorModule = null;

		if (spawn) {
			// Spawn a new one
			Transform clone = Instantiate (modulePrefab, initialPosition, Quaternion.identity) as Transform;
			clone.name = moduleRootName + "_0";
			clone.SetParent (robot.transform);
			clone.GetComponent <ModuleModeController> ().SetTrigger (false);
			initialRotation = clone.GetComponent<ModuleRefPointerController> ().GetPartPointerByName (ModuleRefPointerController.PartNames.BackPlate.ToString ()).transform.rotation;
			
			SetAnchorModule (clone.gameObject);
			
			ma2MaComManager.robotManager.currentConfigurationID = System.Guid.NewGuid ().ToString ();
		}
		ma2MaComManager.behaviorManager.OnClickClear ();
		ma2MaComManager.robotManager.currentConfigurationID = System.Guid.NewGuid ().ToString ();
    }

    public void ResetModulePositions () {
        if (anchorModule == null) {
            anchorModule = robot.transform.GetChild (0).gameObject;
        }

        Vector3 offset = anchorModule.GetComponent<ModuleRefPointerController> ().GetPartPointerByName (ModuleRefPointerController.PartNames.BackPlate.ToString ()).transform.position - initialPosition;
        Quaternion angleOffset = Quaternion.Inverse (initialRotation)
            * anchorModule.GetComponent<ModuleRefPointerController> ().GetPartPointerByName (ModuleRefPointerController.PartNames.BackPlate.ToString ()).transform.rotation;
        foreach (Transform module in robot) {
            foreach (GameObject part in module.GetComponent<ModuleRefPointerController> ().GetAllPartPointers ()) {
                part.transform.position -= offset;
                part.transform.rotation *= angleOffset;
            }
        }
    }

    void RemoveAllModules () {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in robot.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    public void ResetAllModulePositions () {

    }

    public void DeleteSelectedModule () {
        if (ma2MaComManager.selectionManager.selectedModule) {
            DeleteModule (ma2MaComManager.selectionManager.selectedModule);
            ma2MaComManager.selectionManager.ResetSelectedModule ();
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.ResetTextMessage ();
			ma2MaComManager.robotManager.currentConfigurationID = System.Guid.NewGuid ().ToString ();
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

    public Transform InsertModuleAt (Vector3 position, Quaternion rotation, GameObject newRobot = null, string name = "") {
        Transform clone = Instantiate (modulePrefab, position, rotation) as Transform;
        if (name == "") {
            clone.name = FindNextAvailableName ();
        }
        else {
            clone.name = name;
        }

        if (newRobot == null) {
            clone.parent = robot.transform;
        }
        else {
            clone.parent = newRobot.transform;
        }

        return clone;
    }

    public string FindNextAvailableName () {
        int i = 0;
        while (robot.Find (moduleRootName + "_" + i.ToString ()) != null) {
            i++;
        }
        return moduleRootName + "_" + i.ToString ();
    }

    public void ShowConnectionsOrNot (bool show) {
        foreach (Transform m in robot) {
            m.GetComponent<ModuleConnectionController> ().ShowConnectionsOrNot (show);
        }

        if (ma2MaComManager.robotManager.newRobot != null) {
            foreach (Transform m in ma2MaComManager.robotManager.newRobot.transform) {
                m.GetComponent<ModuleConnectionController> ().ShowConnectionsOrNot (show);
            }
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

    public void SetAllModuleStateObjects (List<ModuleStateObject> listOfModuleStateObjects, bool reset = false) {
        foreach (ModuleStateObject mso in listOfModuleStateObjects) {
            GameObject m = FindModuleWithName (mso.name);
            if (m != null) {
				m.GetComponent<ModuleMotionController> ().SetModuleStateObject (mso, reset);
            }
            else {
                Debug.Log ("Cannot find module with name " + mso.name);
            }
        }
    }

	public GameObject FindModuleWithName (string name, Transform robotPointer) {
		foreach (Transform m in robotPointer) {
			if (m.name == name) {
				return m.gameObject;
			}
		}
		return null;
	}

    public GameObject FindModuleWithName (string name) {
        return FindModuleWithName (name, robot);
    }

    public GameObject FindModuleWithNameInNewRobot (string name) {
        foreach (Transform m in ma2MaComManager.robotManager.newRobot.transform) {
            if (m.name == name) {
                return m.gameObject;
            }
        }
        return null;
    }

    public void SetAllModuleMode (ModuleModeController.ModuleMode mode) {
        foreach (Transform m in robot) {
            m.GetComponent<ModuleModeController> ().SetMode (mode);
        }

        if (mode == ModuleModeController.ModuleMode.Edit) {
            anchorModule.GetComponent<ModuleModeController> ().SetAnchorOrNot (true);
        }
    }

    public void SpawnModules (List<ModuleStateObject> listOfModuleStateObjects, GameObject newRobot = null) {
        foreach (ModuleStateObject mso in listOfModuleStateObjects) {
            GameObject m = InsertModuleAt (mso.position, mso.rotation, newRobot, mso.name).gameObject;
            m.transform.Rotate (Vector3.right, 90.0f);
            m.GetComponent <ModuleMotionController> ().SetModuleStateObject (mso);
        }
    }

}
