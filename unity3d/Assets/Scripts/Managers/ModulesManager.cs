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
//        connectionTable = new Hashtable ();
//        UIManagerScript.SetSelectedModule (null);

        // Spawn a new one
        Transform clone = Instantiate (modulePrefab, initialPosition, Quaternion.identity) as Transform;
        clone.name = moduleRootName + "_0";
        clone.SetParent (robot.transform);
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
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTextMessage ("Please select a module before click \"Delete\".");
        }
    }

    public void DeleteModule (GameObject module) {
        //TODO: Remove connection too
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

}
