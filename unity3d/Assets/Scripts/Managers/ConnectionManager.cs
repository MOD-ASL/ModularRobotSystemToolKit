using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;

	// Use this for initialization
	void Start () {
	
	}
	
    void Awake () {
        ma2MaComManager = gameObject.GetComponent<Ma2MaComManager> ();
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void ConnectModule2Node (GameObject module, GameObject node) {
        GameObject nodeOnModule = node.GetComponent<PartController> ().touchedNode;

        Connect (nodeOnModule, node);
        Connect (node, nodeOnModule);

        FixedJoint fj = nodeOnModule.AddComponent<FixedJoint> ();
        fj.connectedBody = node.GetComponent<Rigidbody> ();
    }

    void Connect (GameObject node1, GameObject node2) {
        node1.transform.parent.GetComponent<ModuleConnectionController> ().Connect (node1, node2);
    }
}
