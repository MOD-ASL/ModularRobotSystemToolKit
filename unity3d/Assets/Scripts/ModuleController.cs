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

	public float GetJointValue (string jointName) {
		if (jointName == "centerJoint") return centerJoint.spring.targetPosition;
		else if (jointName == "leftJoint") return leftJoint.spring.targetPosition;
		else if (jointName == "rightJoint") return rightJoint.spring.targetPosition;
		else return frontJoint.spring.targetPosition;
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

	public void UpdateLeftJointAngle (float jointValue) {
		leftWheel.GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = leftJoint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = jointValue;
		leftJoint.spring = spring;
		leftJoint.useSpring = true;
	}

	public void UpdateRightJointAngle (float jointValue) {
		rightWheel.GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = rightJoint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = jointValue;
		rightJoint.spring = spring;
		rightJoint.useSpring = true;
	}

	public void UpdateFrontJointAngle (float jointValue) {
		frontWheel.GetComponent<Rigidbody> ().WakeUp ();
		JointSpring spring = frontJoint.spring;
		spring.spring = 10000.0f;
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
}
