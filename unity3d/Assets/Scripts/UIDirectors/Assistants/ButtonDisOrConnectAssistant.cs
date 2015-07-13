using UnityEngine;
using System.Collections;

public class ButtonDisOrConnectAssistant : MonoBehaviour {

    private GameObject selectedNode1;
    private GameObject selectedNode2;

    public Ma2MaComManager ma2MaComManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//    public void SelectNode (GameObject node) {
//        if (selectedNode1 == null) {
//            selectedNode1 = node;
//            selectedNode1.GetComponent<PartInteractionController> ().SelectOrNot (true);
//            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Please select the second node");
//        }
//        else if ((selectedNode2 == null) && (selectedNode1 != node)) {
//            selectedNode2 = node;
//            selectedNode2.GetComponent<PartInteractionController> ().SelectOrNot (true);
//        }
//        else {
//            selectedNode1.GetComponent<PartInteractionController> ().SelectOrNot (false);
//            selectedNode2.GetComponent<PartInteractionController> ().SelectOrNot (false);
//            selectedNode1 = node;
//            selectedNode2 = null;
//            selectedNode1.GetComponent<PartInteractionController> ().SelectOrNot (true);
//            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Please select the second node");
//        }
//    }
}
