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

    public void ShowConnectionsOrNot (bool show) {
        foreach (KeyValuePair<GameObject, GameObject> kvp in connectionDict) {
            kvp.Key.GetComponent<PartInteractionController> ().ShowConnectionsOrNot (show, (kvp.Value != null));
        }
    }

    public void ResetConnectionDict () {
        connectionDict.Clear ();
        foreach (GameObject node in mo2MaComController.moduleRefPointerController.GetAllNodePointers ()) {
            connectionDict.Add (node, null);
            Destroy (node.GetComponent<FixedJoint> ());
        }
    }

    public void DisconnectAllNodes () {
        foreach (KeyValuePair<GameObject, GameObject> kvp in connectionDict) {
            // Tell other connected nodes to disconnect too
            if (kvp.Value != null) {
                kvp.Value.transform.parent.GetComponent<ModuleConnectionController> ().DisconnectAndReturnOtherNode (kvp.Value);
            }
        }

        ResetConnectionDict ();
    }

    public void ConnectAllNodes () {
        foreach (GameObject node in mo2MaComController.moduleRefPointerController.GetAllNodePointers ()) {
            if (IsNodeAvailable (node)) {
                GameObject touchedNode = node.GetComponent<PartController> ().touchedNode;
                if (touchedNode) {
                    mo2MaComController.ma2MoComManager.ma2MaComManager.connectionManager.ConnectNode2Node (node, touchedNode);
                }
            }
        }
    }

    public void Connect (GameObject thisNode, GameObject otherNode) {
        connectionDict[thisNode] = otherNode;
    }

    public GameObject DisconnectAndReturnOtherNode (GameObject thisNode) {
        GameObject otherNode = connectionDict[thisNode];
        Destroy (thisNode.GetComponent<FixedJoint> ());
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
