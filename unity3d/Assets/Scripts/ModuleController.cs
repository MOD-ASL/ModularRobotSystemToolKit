using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ModuleController : MonoBehaviour {
	// Variables
	enum jointModeEnum {vel, angle, free};

	IDictionary<string, float> jointVel = new Dictionary<string, float>();
	IDictionary<string, float> jointAngle = new Dictionary<string, float>();
	IDictionary<string, int> jointMode = new Dictionary<string, int>();
	private Vector3 screenPoint;
	private Vector3 offset;
	//private bool dragMode = false;
	// Use this for initialization
	void Start () {
		jointVel.Add("LeftWheel", 0.0f);
		jointVel.Add("RightWheel", 0.0f);
		jointVel.Add("FrontWheel", 0.0f);
		jointVel.Add("BackPlate", 0.0f);

		jointAngle["LeftWheel"] = 0.0f;
		jointAngle["RightWheel"] = 0.0f;
		jointAngle["FrontWheel"] = 0.0f;
		jointAngle["BackPlate"] = 0.0f;

		jointMode.Add("LeftWheel", (int)jointModeEnum.angle);
		jointMode.Add("RightWheel", (int)jointModeEnum.angle);
		jointMode.Add("FrontWheel", (int)jointModeEnum.angle);
		jointMode.Add("BackPlate", (int)jointModeEnum.angle);
	}

	// Update is called once per frame
	void FixedUpdate () {
		UpdateJoints();
	}

	// Set each hinge joint based on their mode
	void UpdateJoints () {
		HingeJoint[] hingJoints;
		hingJoints = gameObject.GetComponentsInChildren<HingeJoint>();
		foreach (HingeJoint joint in hingJoints) {
			if (jointMode[joint.connectedBody.name] == (int)jointModeEnum.angle) {
				SetJointAngle(joint, jointAngle[joint.connectedBody.name]);
				joint.useSpring = true;
				joint.useMotor = !joint.useSpring;
			}
			else if (jointMode[joint.connectedBody.name] == (int)jointModeEnum.vel) {
				SetJointVel(joint, jointVel[joint.connectedBody.name]);
				joint.useSpring = false;
				joint.useMotor = !joint.useSpring;
			}
			else if (jointMode[joint.connectedBody.name] == (int)jointModeEnum.free) {
				joint.useSpring = false;
				joint.useMotor = joint.useSpring;
			}
		}
	}

	// Set the given joint to the given angle position
	void SetJointAngle (HingeJoint joint, float angle) {
		JointSpring spring = joint.spring;
		spring.spring = 30000;
		spring.damper = 3;
		spring.targetPosition = angle;
		joint.spring = spring;
	}

	// Set the given joint to the given angle position
	void SetJointAngleWithLimit (HingeJoint joint, float angle) {
		JointLimits limits = joint.limits;
		limits.min = angle;
		limits.max = angle;
		joint.limits = limits;
	}
	
	// Set the given joint to the given velocity
	void SetJointVel (HingeJoint joint, float vel) {
		JointMotor motor = joint.motor;
		motor.force = 100;
		motor.targetVelocity = vel;
		joint.motor = motor;
	}

	// Get the angle with the given joint name
	float GetJointAngleWithName (string jointName) {
		HingeJoint[] hingJoints;
		hingJoints = gameObject.GetComponentsInChildren<HingeJoint>();
		foreach (HingeJoint joint in hingJoints) {
			if (jointName == joint.connectedBody.name) {
				return joint.angle;
			}
		}
		return 0.0f;
	}

	// Set velocity
	public void SetVel(string jointName, float vel) {
		jointVel[jointName] = vel;
	}
	// Set angle
	public bool SetAngle(string jointName, float angle) {
		jointAngle[jointName] = angle;

		if (Math.Abs(GetJointAngleWithName(jointName) - angle) < 20) {
			return true;
		}
		else {
			return false;
		}
	}
	// Set mode
	public void SetMode(string jointName, int mode) {
		jointMode[jointName] = mode;
	}
}

