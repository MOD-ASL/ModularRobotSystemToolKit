using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleMotionController : MonoBehaviour {

    [HideInInspector]
    public Mo2MaComController mo2MaComController;

    // Name of part joint connected to -> JointCommandObject
    // Possible key: FrontWheel, RightWheel, LeftWheel, Body
    public Dictionary<string, JointCommandObject> jointCommandObjectDict = new Dictionary<string, JointCommandObject>();

    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //transform.parent.position = transform.position - transform.localPosition;
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
		mo2MaComController.moduleRefPointerController.GetJointCommandControllerByName (jointName).ChangeCommandType (JointCommandObject.CommandTypes.Position);
        mo2MaComController.moduleRefPointerController.GetJointCommandControllerByName (jointName).UpdateJointAngle (jointValue);

		jointCommandObjectDict[jointName].commandType = JointCommandObject.CommandTypes.Position;
        jointCommandObjectDict[jointName].targetValue = jointValue;
    }

	// update joint target position with the given value
	public void UpdateJointVelocity (float jointValue, string jointName, float period = 3.0f) {
		mo2MaComController.moduleRefPointerController.GetJointCommandControllerByName (jointName).ChangeCommandType (JointCommandObject.CommandTypes.Velocity);
        if (jointCommandObjectDict[jointName].cmdCoroutine != null) {
            StopCoroutine (jointCommandObjectDict[jointName].cmdCoroutine);
        }
        jointCommandObjectDict[jointName].cmdCoroutine = mo2MaComController.moduleRefPointerController.GetJointCommandControllerByName (jointName).SetJointVelocityAndWait (jointValue, period);
        StartCoroutine (jointCommandObjectDict[jointName].cmdCoroutine);


		jointCommandObjectDict[jointName].commandType = JointCommandObject.CommandTypes.Velocity;
		jointCommandObjectDict[jointName].targetValue = jointValue;
        jointCommandObjectDict[jointName].period = period;
	}

    // update joint target position with the given value
    public void StopJoint (string jointName) {
        if (jointCommandObjectDict[jointName].cmdCoroutine != null) {
            StopCoroutine (jointCommandObjectDict[jointName].cmdCoroutine);
        }
        mo2MaComController.moduleRefPointerController.GetJointCommandControllerByName (jointName).StopJoint ();
    }

    public ModuleStateObject GetModuleStateObject (bool forConfiguration) {
        ModuleStateObject mso = new ModuleStateObject ();
        mso.name = gameObject.name;
        mso.position = mo2MaComController.moduleRefPointerController.GetPartPointerByName (ModuleRefPointerController.PartNames.BackPlate.ToString ()).transform.position;
        mso.rotation = mo2MaComController.moduleRefPointerController.GetPartPointerByName (ModuleRefPointerController.PartNames.BackPlate.ToString ()).transform.rotation;
        foreach (JointCommandObject jco in jointCommandObjectDict.Values) {
            JointCommandObject jcoClone = jco.Clone (forConfiguration);
            mso.listOfJointCommands.Add (jcoClone);
            if (mso.period < jcoClone.period) {
                mso.period = jcoClone.period;
            }
        }
		foreach (Transform part in transform) {
			PartStateObject pso = new PartStateObject ();
			pso.name = part.name;
			pso.position = part.position;
			pso.rotation = part.rotation;
			mso.listOfPartStates.Add (pso);
		}
        return mso;
    }

    public void SetModuleStateObject (ModuleStateObject mso, bool reset = false) {
		if (reset) {
			foreach (PartStateObject pso in mso.listOfPartStates) {
				Transform part = transform.FindChild (pso.name);
				part.position = pso.position;
				part.rotation = pso.rotation;
			}
		}
        foreach (JointCommandObject jco in mso.listOfJointCommands) {
			SetJointFromJointCommandObject (jco, reset);
        }
    }

	public void SetJointFromJointCommandObject (JointCommandObject jco, bool reset = false) {
		if (jco.commandType == JointCommandObject.CommandTypes.Position) {
			UpdateJointAngle (jco.targetValue, jco.name);
		}
		else if (jco.commandType == JointCommandObject.CommandTypes.Velocity) {
            if (reset) {
                StopJoint (jco.name);
            }
            else {
                UpdateJointVelocity (jco.targetValue, jco.name, jco.period);
            }
		}
	}

    public void MoveAllPartsByOffset (Vector3 offset) {
        foreach (GameObject part in mo2MaComController.moduleRefPointerController.GetAllPartPointers ()) {
            part.transform.position += offset;
        }
    }
}
