﻿using UnityEngine;
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

    public IEnumerator ConnectModule2Node (GameObject module, GameObject node) {

        // Wait for one frame for collision to happen
        yield return null;

        GameObject nodeOnModule = node.GetComponent<PartController> ().touchedNode;

        ConnectNode2Node (nodeOnModule, node);
    }

    public void ConnectNode2Node (GameObject node1, GameObject node2) {
               
        Connect (node1, node2);
        Connect (node2, node1);
        
        FixedJoint fj = node1.AddComponent<FixedJoint> ();
        fj.connectedBody = node2.GetComponent<Rigidbody> ();
    }

    void Connect (GameObject node1, GameObject node2) {
        node1.transform.parent.GetComponent<ModuleConnectionController> ().Connect (node1, node2);
    }

    public void ShowConnectionsOrNot (bool show) {
        ma2MaComManager.modulesManager.ShowConnectionsOrNot (show);
    }

    public void OnClickDisOrConnect () {
        ShowConnectionsOrNot (ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("DisOrConnect").status);
    }

    public void ToggleConnection (GameObject node1, GameObject node2) {
        if (node1.transform.parent.GetComponent<ModuleConnectionController> ().IsNodeAvailable (node1)) {
            ConnectNode2Node (node1, node2);
        }
        else {
            node1.transform.parent.GetComponent<ModuleConnectionController> ().DisconnectAndReturnOtherNode (node1);
            node2.transform.parent.GetComponent<ModuleConnectionController> ().DisconnectAndReturnOtherNode (node2);
        }
    }

    public void ConnectAllModules () {
        foreach (Transform m in ma2MaComManager.modulesManager.robot) {
            m.GetComponent<ModuleConnectionController> ().ConnectAllNodes ();
        }
    }

    public void DisconnectAllModules () {
        foreach (Transform m in ma2MaComManager.modulesManager.robot) {
            m.GetComponent<ModuleConnectionController> ().DisconnectAllNodes ();
        }
    }

}
