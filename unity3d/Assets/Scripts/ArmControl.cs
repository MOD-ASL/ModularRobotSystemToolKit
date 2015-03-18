using UnityEngine;
using System.Collections;

public class ArmControl : MonoBehaviour {
	private GameObject w;
	private HingeJoint w1Joint;
	private HingeJoint w2Joint1;
	private HingeJoint w2Joint2;
	private HingeJoint w3Joint;
	private HingeJoint w4Joint;
	private HingeJoint w5Joint;
	private HingeJoint w6Joint;
	private HingeJoint w7Joint;
	private HingeJoint w8Joint;

	float rollCount = 0.0f;
	float pitchCount = 0.0f;
	float gripCount = 0.0f;

	// Use this for initialization
	void Start () {
		w = GameObject.Find("ARMSMORES");
		w1Joint = FindCenterJoint(w);
		w1Joint.useSpring = true;

		w = GameObject.Find("ARMSMORES 2");

		Component[] hingeJoints = w.GetComponentsInChildren<HingeJoint>();
		foreach (HingeJoint joint in hingeJoints) {
			if (joint.connectedBody.gameObject.name == "LeftWheel") {
				w2Joint1 = joint;
			}
			if (joint.connectedBody.gameObject.name == "RightWheel") {
				w2Joint2 = joint;
			}
		}
		
		w2Joint1.useSpring = true;
		w2Joint2.useSpring = true;

		w = GameObject.Find("ARMSMORES 3");
		w3Joint = FindCenterJoint(w);
		w3Joint.useSpring = true;
		w = GameObject.Find("ARMSMORES 4");
		w4Joint = FindCenterJoint(w);
		w4Joint.useSpring = true;
		w = GameObject.Find("ARMSMORES 5");
		w5Joint = FindCenterJoint(w);
		w5Joint.useSpring = true;
		
		w = GameObject.Find("ARMSMORES 6");
		w6Joint = FindCenterJoint(w);
		w6Joint.useSpring = true;
		w = GameObject.Find("ARMSMORES 7");
		w7Joint = FindCenterJoint(w);
		w7Joint.useSpring = true;
		w = GameObject.Find("ARMSMORES 8");
		w8Joint = FindCenterJoint(w);
		w8Joint.useSpring = true;
	}
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.RightArrow)) {
			rollCount = Mathf.Min(rollCount + 0.02f, 30);
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			rollCount = Mathf.Max(rollCount - 0.02f, -30);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			pitchCount = Mathf.Min(pitchCount + 0.02f, 30);
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			pitchCount = Mathf.Max(pitchCount - 0.02f, -45);
		}
		if (Input.GetKey(KeyCode.Space)) {
			gripCount = gripCount + 0.0002f;
		}
	}
	// Update is called once per frame
	void Update () {

		JointSpring spring = w1Joint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = rollCount;
		w1Joint.spring = spring;

		spring = w2Joint1.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = pitchCount;
		w2Joint1.spring = spring;

		spring = w2Joint2.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = pitchCount;
		w2Joint2.spring = spring;

		spring = w3Joint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = 65.0f * Mathf.Sin(gripCount);
		spring.targetPosition = Mathf.Abs(spring.targetPosition);
		w3Joint.spring = spring;

		spring = w4Joint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = 65.0f * Mathf.Sin(gripCount);
		spring.targetPosition = Mathf.Abs(spring.targetPosition);
		w4Joint.spring = spring;

		spring = w5Joint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = 65.0f * Mathf.Sin(gripCount);
		spring.targetPosition = Mathf.Abs(spring.targetPosition);
		w5Joint.spring = spring;

		spring = w6Joint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = 65.0f * Mathf.Sin(gripCount);
		spring.targetPosition = - Mathf.Abs(spring.targetPosition);
		w6Joint.spring = spring;

		spring = w7Joint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = 65.0f * Mathf.Sin(gripCount);
		spring.targetPosition = - Mathf.Abs(spring.targetPosition);
		w7Joint.spring = spring;

		spring = w8Joint.spring;
		spring.spring = 10000.0f;
		spring.damper = 30.0f;
		spring.targetPosition = 65.0f * Mathf.Sin(gripCount);
		spring.targetPosition = - Mathf.Abs(spring.targetPosition);
		w8Joint.spring = spring;
	}

	HingeJoint FindCenterJoint (GameObject module) {
		Component[] hingeJoints;
		hingeJoints = module.GetComponentsInChildren<HingeJoint>();
		foreach (HingeJoint joint in hingeJoints) {
			if (joint.connectedBody.gameObject.name == "Body") {
				return joint;
			}
		}
		return null;
	}
}
