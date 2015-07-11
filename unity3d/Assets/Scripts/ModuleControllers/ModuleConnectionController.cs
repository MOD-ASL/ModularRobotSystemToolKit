using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleConnectionController : MonoBehaviour {

    [HideInInspector]
    public Mo2MaComController mo2MaComController;

    // GameObject of node -> GameObject of node on the other module
    // Possible key: FrontWheel, RightWheel, LeftWheel, BackPlate
    private Dictionary<GameObject, GameObject> connectionDict = new Dictionary<GameObject, GameObject>();

	// Use this for initialization
	void Start () {
        ResetConnectionDict ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        mo2MaComController = gameObject.GetComponent<Mo2MaComController> ();
    }

    public void ResetConnectionDict () {
        connectionDict.Clear ();
        foreach (GameObject node in mo2MaComController.moduleRefPointerController.GetAllNodePointers ()) {
            connectionDict.Add (node, null);
        }
    }

    public void Connect (GameObject thisNode, GameObject otherNode) {
        connectionDict[thisNode] = otherNode;
    }

    public GameObject FindClosestNodeFromGivenNode (GameObject otherNode) {
        float minDistance = -1.0f;
        GameObject closestNode = new GameObject ();
        foreach (GameObject node in mo2MaComController.moduleRefPointerController.GetAllNodePointers ()) {
            Debug.Log (node.name);
            Debug.Log ((node.transform.localPosition));
            if ((minDistance == -1.0f) || ((otherNode.transform.position - node.transform.position).magnitude < minDistance)) {
                minDistance = (otherNode.transform.position - node.transform.position).magnitude;
                closestNode = node;
            }
        }

        return closestNode;
    }

    public GameObject DisconnectAndReturnOtherNode (GameObject thisNode) {
        GameObject otherNode = connectionDict[thisNode];
        connectionDict[thisNode] = null;
        return otherNode;
    }

    public bool IsNodeAvailable (GameObject node) {
        if (!connectionDict.ContainsKey (node)) {
            return false;
        }
        if (connectionDict[node] != null) {
            return false;
        }
        return true;
    }
}
