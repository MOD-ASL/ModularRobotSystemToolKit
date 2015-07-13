using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleMotionController : MonoBehaviour {

    [HideInInspector]
    public Mo2MaComController mo2MaComController;

    // Name of part joint connected to -> JointCommandObject
    // Possible key: FrontWheel, RightWheel, LeftWheel, Body
    private Dictionary<string, JointCommandObject> jointCommandObjectDict = new Dictionary<string, JointCommandObject>();

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        // Ignore collisions among different parts within the module
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider1 in colliders) {
            foreach (Collider collider2 in colliders) {
                if (collider1 != collider2) {
                    Physics.IgnoreCollision(collider1, collider2);
                }
            }
        }

        mo2MaComController = gameObject.GetComponent<Mo2MaComController> ();
        if (jointCommandObjectDict.Count == 0) {
            LoadJointCommandObjectDict ();
        }
    }

    // Load the JointCommandObject of all joints
    void LoadJointCommandObjectDict () {
        foreach (string jointName in mo2MaComController.moduleRefPointerController.GetAllHingeJointNames ()) {
            JointCommandObject jco = new JointCommandObject (jointName);
            jointCommandObjectDict.Add (jointName, jco);
        }
    }

    // return the joint value of the given joint name
    public float GetJointValue (string jointName, bool targetValue = true) {
        return mo2MaComController.moduleRefPointerController.GetJointCommandControllerByName (jointName).GetJointValue (targetValue);
    }

    // update joint target position with the given value
    public void UpdateJointAngle (float jointValue, string jointName) {
        mo2MaComController.moduleRefPointerController.GetJointCommandControllerByName (jointName).UpdateJointAngle (jointValue);
        jointCommandObjectDict[jointName].targetValue = jointValue;
    }

    public ModuleStateObject GetModuleStateObject () {
        ModuleStateObject mso = new ModuleStateObject ();
        mso.name = gameObject.name;
        foreach (JointCommandObject jco in jointCommandObjectDict.Values) {
            mso.listOfJointCommands.Add (jco.Clone ());
        }
        return mso;
    }

    public void SetModuleStateObject (ModuleStateObject mso) {
        foreach (JointCommandObject jco in mso.listOfJointCommands) {
            UpdateJointAngle (jco.targetValue, jco.name);
        }
    }
}
