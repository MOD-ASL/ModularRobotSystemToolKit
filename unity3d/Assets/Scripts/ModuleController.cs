using UnityEngine;
using System.Collections;

public class ModuleController : MonoBehaviour {
	HingeJoint centerJoint;
	HingeJoint leftJoint;
	HingeJoint rightJoint;
	HingeJoint frontJoint;

	GameObject backPlate;
	GameObject body;
	GameObject rightWheel;
	GameObject leftWheel;
	GameObject frontWheel;

	// Use this for initialization
	void Start () {
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateCenterJointAngle (float jointValue) {
		body.GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = centerJoint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = jointValue;
		centerJoint.spring = spring;
		centerJoint.useSpring = true;
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
}
