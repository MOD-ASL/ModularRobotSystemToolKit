using UnityEngine;
using System.Collections;

public class PartController : MonoBehaviour {

    [HideInInspector]
    public GameObject touchedNode;

	// Use this for initialization
	void Start () {
        touchedNode = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnJointBreak(float breakForce) {
        //string otherM = gameObject.GetComponent<FixedJoint> ().connectedBody.transform.parent.name;

        //EditorUtility.DisplayDialog("Undesired module disconnection detected !",
        //                            string.Format ("Module {0} is disconnected from Module {1}", otherM, transform.parent.name), "Ok");
    }

    public void OnTriggerEnter (Collider other) {
        if (other.transform.parent.tag == "Module") {
            touchedNode = other.gameObject;
        }
    }

    public void OnTriggerExit () {
        touchedNode = null;
    }
}
