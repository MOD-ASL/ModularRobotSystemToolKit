using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
	private GameObject w;
	private HingeJoint w1Joint;
	private HingeJoint w2Joint;
	private HingeJoint w3Joint;
	private HingeJoint w4Joint;
	private HingeJoint w5Joint;
	private HingeJoint w6Joint;

	// Use this for initialization
	void Start () {
		w = GameObject.Find("SMORES 9");
		w1Joint = FindJoint(w, "LeftWheel");
		w1Joint.useMotor = true;

		w = GameObject.Find("SMORES 10");
		w2Joint = FindJoint(w, "LeftWheel");
		w2Joint.useMotor = true;

		w = GameObject.Find("SMORES 11");
		w3Joint = FindJoint(w, "LeftWheel");
		w3Joint.useMotor = true;

		w = GameObject.Find("SMORES 15");
		w4Joint = FindJoint(w, "RightWheel");
		w4Joint.useMotor = true;
		
		w = GameObject.Find("SMORES 16");
		w5Joint = FindJoint(w, "RightWheel");
		w5Joint.useMotor = true;
		
		w = GameObject.Find("SMORES 17");
		w6Joint = FindJoint(w, "RightWheel");
		w6Joint.useMotor = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W)) {
			Forward();
		}
		else if (Input.GetKey(KeyCode.S)) {
			Backward();
		}
		else if (Input.GetKey(KeyCode.A)) {
			TurnLeft();
		}
		else if (Input.GetKey(KeyCode.D)) {
			TurnRight();
		}
		if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))){
			Stop();
		}
	}

	void Forward () {
		SetJointVel(w1Joint, 480.0f);
		SetJointVel(w2Joint, 480.0f);
		SetJointVel(w3Joint, 480.0f);
		SetJointVel(w4Joint, 480.0f);
		SetJointVel(w5Joint, 480.0f);
		SetJointVel(w6Joint, 480.0f);
	}

	void Backward () {
		SetJointVel(w1Joint, -480.0f);
		SetJointVel(w2Joint, -480.0f);
		SetJointVel(w3Joint, -480.0f);
		SetJointVel(w4Joint, -480.0f);
		SetJointVel(w5Joint, -480.0f);
		SetJointVel(w6Joint, -480.0f);
	}

	void TurnLeft () {
		SetJointVel(w1Joint, -480.0f);
		SetJointVel(w2Joint, -480.0f);
		SetJointVel(w3Joint, -480.0f);
		SetJointVel(w4Joint, 480.0f);
		SetJointVel(w5Joint, 480.0f);
		SetJointVel(w6Joint, 480.0f);
	}
	
	void TurnRight () {
		SetJointVel(w1Joint, 480.0f);
		SetJointVel(w2Joint, 480.0f);
		SetJointVel(w3Joint, 480.0f);
		SetJointVel(w4Joint, -480.0f);
		SetJointVel(w5Joint, -480.0f);
		SetJointVel(w6Joint, -480.0f);
	}

	void Stop () {
		SetJointVel(w1Joint, 0.0f);
		SetJointVel(w2Joint, 0.0f);
		SetJointVel(w3Joint, 0.0f);
		SetJointVel(w4Joint, 0.0f);
		SetJointVel(w5Joint, 0.0f);
		SetJointVel(w6Joint, 0.0f);
	}

	void SetJointVel (HingeJoint hinge, float vel) {
		JointMotor motor = hinge.motor;
		motor.force = 10000.0f;
		motor.targetVelocity = vel;
		hinge.motor = motor;
	}

	HingeJoint FindJoint (GameObject module, string connectedBodyName) {
		Component[] hingeJoints;
		hingeJoints = module.GetComponentsInChildren<HingeJoint>();
		foreach (HingeJoint joint in hingeJoints) {
			if (joint.connectedBody.gameObject.name == connectedBodyName) {
				return joint;
			}
		}
		return null;
	}
}
