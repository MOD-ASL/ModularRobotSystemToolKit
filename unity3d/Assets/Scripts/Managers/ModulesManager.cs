using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulesManager : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;

    public Transform modulePrefab;
    public Transform dummyModulePrefab;
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

}
