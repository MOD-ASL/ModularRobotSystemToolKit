using UnityEngine;
using System.Collections;

public class ModuleController : MonoBehaviour {
	HingeJoint centerJoint;
	HingeJoint leftJoint;
	HingeJoint rightJoint;
	HingeJoint frontJoint;

	public GameObject backPlate;
	public GameObject body;
	public GameObject rightWheel;
	public GameObject leftWheel;
	public GameObject frontWheel;

	public int currentMode; // 0 for edit mode with fixed backPlate. 1 for edit mode. 2 for simulation mode

	// Use this for initialization
	void Start () {

	}

	void Awake () {
		Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
		foreach (Collider collider1 in colliders) {
			foreach (Collider collider2 in colliders) {
				if (collider1 != collider2) {
					Physics.IgnoreCollision(collider1, collider2);
				}
			}
		}

		LoadJointsPointer ();
		LoadGameObjects ();
		SetMode (0);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetMode (int mode) {
		currentMode = mode;
		UpdateModuleFromMode ();
	}

	public void UpdateJointsFromModule (GameObject module) {
		UpdateCenterJointAngle (module.GetComponent<ModuleController> ().centerJoint.spring.targetPosition);
		UpdateLeftJointAngle (module.GetComponent<ModuleController> ().leftJoint.spring.targetPosition);
		UpdateRightJointAngle (module.GetComponent<ModuleController> ().rightJoint.spring.targetPosition);
		UpdateFrontJointAngle (module.GetComponent<ModuleController> ().frontJoint.spring.targetPosition);
	}

	void UpdateModuleFromMode () {
		if (currentMode == 0) {
			body.GetComponent<Rigidbody> ().isKinematic = false;
			body.GetComponent<Rigidbody> ().useGravity = false;
			leftWheel.GetComponent<Rigidbody> ().isKinematic = false;
			leftWheel.GetComponent<Rigidbody> ().useGravity = false;
			rightWheel.GetComponent<Rigidbody> ().isKinematic = false;
			rightWheel.GetComponent<Rigidbody> ().useGravity = false;
			frontWheel.GetComponent<Rigidbody> ().isKinematic = false;
			frontWheel.GetComponent<Rigidbody> ().useGravity = false;
			backPlate.GetComponent<Rigidbody> ().isKinematic = true;
			backPlate.GetComponent<Rigidbody> ().useGravity = false;
		}
		else if (currentMode == 1) {
			body.GetComponent<Rigidbody> ().isKinematic = false;
			body.GetComponent<Rigidbody> ().useGravity = false;
			leftWheel.GetComponent<Rigidbody> ().isKinematic = false;
			leftWheel.GetComponent<Rigidbody> ().useGravity = false;
			rightWheel.GetComponent<Rigidbody> ().isKinematic = false;
			rightWheel.GetComponent<Rigidbody> ().useGravity = false;
			frontWheel.GetComponent<Rigidbody> ().isKinematic = false;
			frontWheel.GetComponent<Rigidbody> ().useGravity = false;
			backPlate.GetComponent<Rigidbody> ().isKinematic = false;
			backPlate.GetComponent<Rigidbody> ().useGravity = false;
		}
		else {
			body.GetComponent<Rigidbody> ().isKinematic = false;
			body.GetComponent<Rigidbody> ().useGravity = true;
			leftWheel.GetComponent<Rigidbody> ().isKinematic = false;
			leftWheel.GetComponent<Rigidbody> ().useGravity = true;
			rightWheel.GetComponent<Rigidbody> ().isKinematic = false;
			rightWheel.GetComponent<Rigidbody> ().useGravity = true;
			frontWheel.GetComponent<Rigidbody> ().isKinematic = false;
			frontWheel.GetComponent<Rigidbody> ().useGravity = true;
			backPlate.GetComponent<Rigidbody> ().isKinematic = false;
			backPlate.GetComponent<Rigidbody> ().useGravity = true;
		}
	}

	public float GetJointValue (string jointName) {
		if (jointName == "centerJoint") return centerJoint.spring.targetPosition;
		else if (jointName == "leftJoint") return leftJoint.spring.targetPosition;
		else if (jointName == "rightJoint") return rightJoint.spring.targetPosition;
		else return frontJoint.spring.targetPosition;
	}

	public void UpdateCenterJointAngle (float jointValue) {
		body.GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = centerJoint.spring;
		spring.spring = 100000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = jointValue;
		centerJoint.spring = spring;
		centerJoint.useSpring = true;
	}

	public void UpdateLeftJointAngle (float jointValue) {
		leftWheel.GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = leftJoint.spring;
		spring.spring = 100000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = jointValue;
		leftJoint.spring = spring;
		leftJoint.useSpring = true;
	}

	public void UpdateRightJointAngle (float jointValue) {
		rightWheel.GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = rightJoint.spring;
		spring.spring = 100000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = jointValue;
		rightJoint.spring = spring;
		rightJoint.useSpring = true;
	}

	public void UpdateFrontJointAngle (float jointValue) {
		frontWheel.GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = frontJoint.spring;
		spring.spring = 100000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = jointValue;
		frontJoint.spring = spring;
		frontJoint.useSpring = true;
	}

	// Load the pointers of all joints for easier reference
	void LoadJointsPointer () {
		HingeJoint[] joints = gameObject.GetComponentsInChildren<HingeJoint> ();
		foreach (HingeJoint joint in joints) {
			if (joint.connectedBody.name == "Body") centerJoint = joint;
			else if (joint.connectedBody.name == "RightWheel") rightJoint = joint;
			else if (joint.connectedBody.name == "LeftWheel") leftJoint = joint;
			else if (joint.connectedBody.name == "FrontWheel") frontJoint = joint;
		}
	}

	// Load the pointers of all chile game object for easier reference
	void LoadGameObjects () {
		foreach (Transform child in transform) {
			if (child.name == "Body") body = child.gameObject;
			else if (child.name == "BackPlate") backPlate = child.gameObject;
			else if (child.name == "LeftWheel") leftWheel = child.gameObject;
			else if (child.name == "RightWheel") rightWheel = child.gameObject;
			else if (child.name == "FrontWheel") frontWheel = child.gameObject;
		}
	}

	public void OnSelected () {
		foreach (Transform child in transform) {
			Renderer rend = child.GetComponent<Renderer> ();
			rend.material.color = new Color (0.3f, 1.0f, 0.3f);
		}
	}

	public void OnDeselected () {
		foreach (Transform child in transform) {
			Renderer rend = child.GetComponent<Renderer> ();
			rend.material.color = new Color (0.6f, 0.6f, 0.6f);
		}
	}

	public void OnHighlighted (bool hightlight) {
		if (gameObject.tag == "Ghost") {
			foreach (Transform child in transform) {
				Renderer rend = child.GetComponent<Renderer> ();
				if (hightlight) {
					rend.material.color = new Color (0.8f, 0.8f, 0.0f);
				}
				else {
					rend.material.color = new Color (0.9f, 1.0f, 0.35f);
				}
			}
		}
	}

	public void OnLite (bool lite) {

		foreach (Transform child in transform) {
			Renderer rend = child.GetComponent<Renderer> ();
			if (lite) {
				rend.material.color = new Color (1.0f, 1.0f, 1.0f);
			}
			else {
				rend.material.color = new Color (0.6f, 0.6f, 0.6f);
			}
		}

	}


	public GameObject[] GetAllNodes () {
		GameObject[] nodes = new GameObject[4];
		nodes[0] = backPlate;
		nodes[1] = leftWheel;
		nodes[2] = rightWheel;
		nodes[3] = frontWheel;
		return nodes;
	}

	public void SetToTrigger (bool trigger) {
		foreach (Transform child in transform) {
			Collider c = child.GetComponent<Collider> ();
			c.isTrigger = trigger;
		}
	}
}

