﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleController : MonoBehaviour {

	// name of part joint connected to -> HingeJoint
	public Dictionary<string, HingeJoint> jointsHashTable = new Dictionary<string, HingeJoint>();
	// part name -> GameObject of part
	public Dictionary<string, GameObject> partsHashTable = new Dictionary<string, GameObject>();
	// node name -> GameObject of part
	public Dictionary<string, GameObject> nodesHashTable = new Dictionary<string, GameObject>();

	ColorManager colorManager;

	public int currentMode; // 0 for edit mode with fixed backPlate. 1 for edit mode. 2 for simulation mode

	// constant
	float jointSpringForce = 100000.0f;
	float jointDamperForce = 30.0f;

	// define enum for part name
	public enum PartNames {BackPlate, Body, RightWheel, LeftWheel, FrontWheel};
	
	// Use this for initialization
	void Start () {

	}

	void Awake () {
		// ignore collisions among different parts within the module
		Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
		foreach (Collider collider1 in colliders) {
			foreach (Collider collider2 in colliders) {
				if (collider1 != collider2) {
					Physics.IgnoreCollision(collider1, collider2);
				}
			}
		}

		// load color manager
		colorManager = (ColorManager) GameObject.FindObjectOfType<ColorManager> ();

		// load pointers
		LoadJointPointers ();
		LoadPartPointers ();
		LoadNodePointers ();

		// set of the mode for the module
		SetMode (0);
	}
	
	// Update is called once per frame
	void Update () {

	}

	// set the mode and update the module accordingly
	public void SetMode (int mode) {
		currentMode = mode;
		UpdateModuleFromMode ();
	}

	// update the joint target positions based on the corresponding joint from the given module
	public void UpdateJointsFromModule (GameObject module) {
		foreach (string jointName in jointsHashTable.Keys) {
			UpdateJointAngle (module.GetComponent<ModuleController> ().GetJointValue (jointName), jointName);
		}
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

	void UpdateModuleFromMode () {
		SetKinematic (false);
		if (currentMode == 0) {
			SetGravity (false);
			partsHashTable[PartNames.BackPlate.ToString ()].GetComponent<Rigidbody> ().isKinematic = true;
		}
		else if (currentMode == 1) {
			SetGravity (false);
		}
		else if (currentMode == 2) {
			SetGravity (true);
		}
	}

	// return the joint value of the given joint name
	public float GetJointValue (string jointName) {
		return jointsHashTable[jointName].spring.targetPosition;
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
			}
		}
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
}

