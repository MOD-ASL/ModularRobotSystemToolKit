using UnityEngine;
using System.Collections;
using UnityEditor;

public class PartController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnJointBreak(float breakForce) {
        string otherM = gameObject.GetComponent<FixedJoint> ().connectedBody.transform.parent.name;

        EditorUtility.DisplayDialog("Undesired module disconnection detected !",
                                    string.Format ("Module {0} is disconnected from Module {1}", otherM, transform.parent.name), "Ok");
    }
}
