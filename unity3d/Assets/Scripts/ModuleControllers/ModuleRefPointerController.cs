using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleRefPointerController : MonoBehaviour {

    // Define enum for part name
    public enum PartNames {BackPlate, Body, RightWheel, LeftWheel, FrontWheel};

    // Name of part joint connected to -> HingeJoint
    // Possible key: FrontWheel, RightWheel, LeftWheel, Body
    private Dictionary<string, HingeJoint> jointPointerDict = new Dictionary<string, HingeJoint>();
    
    // Part name -> GameObject of part
    // Possible key: FrontWheel, RightWheel, LeftWheel, Body, BackPlate
    private Dictionary<string, GameObject> partPointerDict = new Dictionary<string, GameObject>();
    
    // Node name -> GameObject of part
    // Possible key: FrontWheel, RightWheel, LeftWheel, BackPlate
    private Dictionary<string, GameObject> nodePointerDict = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        // Load pointers
        LoadJointPointers ();
        LoadPartPointers ();
        LoadNodePointers ();
    }

    // Load the pointers of all joints for easier reference
    void LoadJointPointers () {
        HingeJoint[] joints = gameObject.GetComponentsInChildren<HingeJoint> ();
        foreach (HingeJoint joint in joints) {
            jointPointerDict.Add (joint.connectedBody.name, joint);
        }
    }
    
    // Load the pointers of all part game object for easier reference
    void LoadPartPointers () {
        foreach (Transform child in transform) {
            partPointerDict.Add (child.name, child.gameObject);
        }
    }
    
    // Load the pointers of all node game object for easier reference
    void LoadNodePointers () {
        foreach (Transform child in transform) {
            if (child.name != "Body") {
                nodePointerDict.Add (child.name, child.gameObject);
            }
        }
    }

    public HingeJoint GetHingeJointPointerByName (string name) {
        return jointPointerDict[name];
    }

    public GameObject GetPartPointerByName (string name) {
        return partPointerDict[name];
    }

    public GameObject GetNodePointerByName (string name) {
        return nodePointerDict[name];
    }

    public List<string> GetAllHingeJointNames () {
        return new List<string> (jointPointerDict.Keys);
    }

    public List<string> GetAllPartNames () {
        return new List<string> (partPointerDict.Keys);
    }

    public List<string> GetAllNodeNames () {
        return new List<string> (nodePointerDict.Keys);
    }

    public List<HingeJoint> GetAllHingeJointPointers () {
        return new List<HingeJoint> (jointPointerDict.Values);
    }
    
    public List<GameObject> GetAllPartPointers () {
        return new List<GameObject> (partPointerDict.Values);
    }
    
    public List<GameObject> GetAllNodePointers () {
        return new List<GameObject> (nodePointerDict.Values);
    }

}
