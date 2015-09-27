using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleController : MonoBehaviour {

	// Name of part joint connected to -> HingeJoint
    // Possible key: FrontWheel, RightWheel, LeftWheel, Body
	public Dictionary<string, HingeJoint> jointsHashTable = new Dictionary<string, HingeJoint>();

	// Part name -> GameObject of part
    // Possible key: FrontWheel, RightWheel, LeftWheel, Body, BackPlate
	public Dictionary<string, GameObject> partsHashTable = new Dictionary<string, GameObject>();

	// Node name -> GameObject of part
    // Possible key: FrontWheel, RightWheel, LeftWheel, BackPlate
	public Dictionary<string, GameObject> nodesHashTable = new Dictionary<string, GameObject>();

	// Game object of part -> connected GameObject of part
    // Only node can connect to each other
	public Dictionary<GameObject, GameObject> nodesConnectionHashTable = new Dictionary<GameObject, GameObject>();

    // Avaliable modes
    // Static mode: BackPlate is fixed in place, no gravity
    // Edit mode: No gravity
    // Simulate mode: Full dynamics with collision
    enum Mode {Static, Edit, Simulation};
	// currentMode of the module
    int currentMode;

	// constant
	float jointSpringForce = 100000.0f;
	public float jointDamperForce = 3000.0f; //TODO: make this private

	// define enum for part name
	public enum PartNames {BackPlate, Body, RightWheel, LeftWheel, FrontWheel};

    ColorManager colorManager; //TODO: How to manage color?
    public Camera sensor;

    // ----------------------------- Start of methods ----------------------------- //

	// Use this for initialization
	void Start () {

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

		// Load color manager
		colorManager = (ColorManager) GameObject.FindObjectOfType<ColorManager> ();

		// Load pointers
		LoadJointPointers ();
		LoadPartPointers ();
		LoadNodePointers ();

		// Set of the mode for the module
		SetMode (0);
	}
	
	// Update is called once per frame
	void Update () {
        if (sensor != null) {
            sensor.transform.position = partsHashTable[PartNames.FrontWheel.ToString ()].transform.position;
            sensor.transform.rotation = partsHashTable[PartNames.FrontWheel.ToString ()].transform.rotation;
            sensor.transform.RotateAround (sensor.transform.position, sensor.transform.up, 90.0f);
            sensor.transform.RotateAround (sensor.transform.position, sensor.transform.forward, 90.0f);
        }
	}

    public void AttachSensor (Camera s) {
        sensor = s;
    }

    public void ResetSensor () {
        sensor = null;
    }

	// Set the mode and update the module accordingly
	public void SetMode (int mode) {
		currentMode = mode;
		UpdateModuleFromMode ();
	}

    // Update physics setting based on the mode
    void UpdateModuleFromMode () {
        SetKinematic (false);
        if (currentMode == 0) {
            // Static mode: BackPlate is fixed in place, no gravity
            SetGravity (false);
            partsHashTable[PartNames.BackPlate.ToString ()].GetComponent<Rigidbody> ().isKinematic = true;
        }
        else if (currentMode == 1) {
            // Edit mode: No gravity
            //TODO: should settrigger be here?
            SetGravity (false);
        }
        else if (currentMode == 2) {
            // Simulate mode: Full dynamics with collision
            SetGravity (true);
        }
    }

	// Update the joint target positions based on the corresponding joint from the given module
	public void UpdateJointsFromModule (GameObject module) {
		foreach (string jointName in jointsHashTable.Keys) {
			UpdateJointAngle (module.GetComponent<ModuleController> ().GetJointValue (jointName), jointName);
		}
	}

    // Update the joint target positions based on the corresponding joint from the given module state
    public bool UpdateJointsFromModuleState (ModuleState mState) {
        bool reached;
        UpdateJointAngle (mState.jointAngles[0], PartNames.FrontWheel.ToString ());
        reached = (Mathf.Abs (mState.jointAngles[0] - GetJointValue (PartNames.FrontWheel.ToString (), false)) < 2f);
        UpdateJointAngle (mState.jointAngles[1], PartNames.LeftWheel.ToString ());
        reached = (Mathf.Abs (mState.jointAngles[1] - GetJointValue (PartNames.LeftWheel.ToString (), false)) < 2f);
        UpdateJointAngle (mState.jointAngles[2], PartNames.RightWheel.ToString ());
        reached = (Mathf.Abs (mState.jointAngles[2] - GetJointValue (PartNames.RightWheel.ToString (), false)) < 2f);
        UpdateJointAngle (mState.jointAngles[3], PartNames.Body.ToString ());
        reached = (Mathf.Abs (mState.jointAngles[3] - GetJointValue (PartNames.Body.ToString (), false)) < 2f);

        return reached;
    }

	// set all parts to be kinematic or not
	void SetKinematic (bool kinematic) {
		foreach (GameObject part in partsHashTable.Values) {
			part.GetComponent<Rigidbody> ().isKinematic = kinematic;
		}
	}

	// set all parts to use gravity or not
	void SetGravity (bool gravity) {
		foreach (GameObject part in partsHashTable.Values) {
			part.GetComponent<Rigidbody> ().useGravity = gravity;
		}
	}

	

	// return the joint value of the given joint name
	public float GetJointValue (string jointName, bool targetValue = true) {
        if (targetValue) {
            return jointsHashTable[jointName].spring.targetPosition;
        }
        return jointsHashTable[jointName].angle;
	}

	// update joint target position with the given value
	public void UpdateJointAngle (float jointValue, string jointName) {
		partsHashTable[jointName].GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = jointsHashTable[jointName].spring;
		spring.spring = jointSpringForce;
		spring.damper = jointDamperForce;
		spring.targetPosition = jointValue;
		jointsHashTable[jointName].spring = spring;
		jointsHashTable[jointName].useSpring = true;
	}

	// update the angle of center joint (for slider callback)
	public void UpdateCenterJointAngle (float jointValue) {
		UpdateJointAngle (jointValue, PartNames.Body.ToString ());
	}

	// update the angle of left joint (for slider callback)
	public void UpdateLeftJointAngle (float jointValue) {
		UpdateJointAngle (jointValue, PartNames.LeftWheel.ToString ());
	}

	// update the angle of right joint (for slider callback)
	public void UpdateRightJointAngle (float jointValue) {
		UpdateJointAngle (jointValue, PartNames.RightWheel.ToString ());
	}

	// update the angle of front joint (for slider callback)
	public void UpdateFrontJointAngle (float jointValue) {
		UpdateJointAngle (jointValue, PartNames.FrontWheel.ToString ());
	}
	
	// Load the pointers of all joints for easier reference
	void LoadJointPointers () {
		HingeJoint[] joints = gameObject.GetComponentsInChildren<HingeJoint> ();
		foreach (HingeJoint joint in joints) {
			jointsHashTable.Add (joint.connectedBody.name, joint);
		}
	}

	// Load the pointers of all child game object for easier reference
	void LoadPartPointers () {
		foreach (Transform child in transform) {
			partsHashTable.Add (child.name, child.gameObject);
		}
	}

	// Load the pointers of all node game object for easier reference
	void LoadNodePointers () {
		foreach (Transform child in transform) {
			if (child.name != "Body") {
				nodesHashTable.Add (child.name, child.gameObject);
				nodesConnectionHashTable.Add (child.gameObject, null);
			}
		}
	}

	// connect node
	public void OnConnectNode (GameObject thisNode, GameObject connectedNode) {
		nodesConnectionHashTable[thisNode] = connectedNode;
	}

	// disconnect node
	public void OnDisconnectNode (GameObject thisNode) {
		nodesConnectionHashTable[thisNode] = null;
	}

	// change the color when the module is selected or not
	public void OnSelected (bool select) {
		if (select) {
			ChangeColor (colorManager.moduleSelected);
		}
		else {
			ChangeColor (colorManager.moduleNormal);
		}
	}

	// change the color when the module is highlighted or not
	public void OnHighlighted (bool hightlight) {
		if (gameObject.tag == "Ghost") {
			if (hightlight) {
				ChangeColor (colorManager.moduleHighlighted);
			}
			else {
				ChangeColor (colorManager.moduleGhost);
			}
		}
	}

	// change the color when the module is in lite mode or not
	public void OnLite (bool lite) {
		if (lite) {
			ChangeColor (colorManager.moduleLite);
		}
		else {
			ChangeColor (colorManager.moduleNormal);
		}
	}

	// change the color when the module is in connection mode or not
	public void OnShowConnection (bool showConnection) {
		if (showConnection) {
			foreach (GameObject thisNode in nodesConnectionHashTable.Keys) {
				if (nodesConnectionHashTable[thisNode] != null) {
					ChangeColor (colorManager.nodeConnected, thisNode);
				}
				else {
					ChangeColor (colorManager.nodeDisconnected, thisNode);
				}
			}
		}
		else {
			ChangeColor (colorManager.moduleNormal);
		}
	}

	// set all parts to trigger or not
	public void SetToTrigger (bool trigger) {
		foreach (Transform child in transform) {
			Collider c = child.GetComponent<Collider> ();
			c.isTrigger = trigger;
		}
	}

	// change the color of all parts
	void ChangeColor (Color c) {
		foreach (Transform child in transform) {
			Renderer rend = child.GetComponent<Renderer> ();
			rend.material.color = c;
		}
	}

	// change the color of the given part parts
	void ChangeColor (Color c, GameObject part) {
		Renderer rend = part.GetComponent<Renderer> ();
		rend.material.color = c;
	}

    // Get all joint angles of the module
    // The return float array is in format of "FrontWheel, LeftWheel, RightWheel, Body"
    public float[] GetJointAnglesInArray () {
        float[] jointAngles = new float[4];
        int count = 0;
        foreach (string jName in new string[4] {PartNames.FrontWheel.ToString (),
                                                PartNames.LeftWheel.ToString (), 
                                                PartNames.RightWheel.ToString (), 
                                                PartNames.Body.ToString ()}) {
            jointAngles[count] = jointsHashTable[jName].spring.targetPosition;
            count++;
        }
        return jointAngles;
    }

    // This method is for saving the state of the module to a file
    // Get the position and orientation of the module
    // The position and orientation of a module is defined by the BackPlate
    // The return string is in format of "px py pz qw qx qy qz"
    public string GetPositionInString () {
        Vector3 position = partsHashTable[PartNames.BackPlate.ToString ()].transform.position;
        Quaternion q = partsHashTable[PartNames.BackPlate.ToString ()].transform.rotation;
        
        return string.Format ("{0,1:0.000} {1,1:0.000} {2,1:0.000} {3,1:0.000} {4,1:0.000} {5,1:0.000} {6,1:0.000}", 
                              position.x, position.y, position.z, q.w, q.x, q.y, q.z);
    }

    // This method is for saving the state of the module to a file
    // Get all joint angles of the module in one string
    // The return string is in format of "FrontWheel, LeftWheel, RightWheel, Body"
    public string GetJointAnglesInString () {
        string[] jointAnglesInString = new string[4];
        int count = 0;
        foreach (string jName in new string[4] {PartNames.FrontWheel.ToString (),
                                                PartNames.LeftWheel.ToString (), 
                                                PartNames.RightWheel.ToString (), 
                                                PartNames.Body.ToString ()}) {
            jointAnglesInString[count] = string.Format ("{0,1:0.000}", jointsHashTable[jName].spring.targetPosition);
            count++;
        }
        return string.Join (" ", jointAnglesInString);
    }
}

