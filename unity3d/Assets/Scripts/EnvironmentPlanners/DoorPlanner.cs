using UnityEngine;
using System.Collections;

public class DoorPlanner : MonoBehaviour {

	public Animator doorAnimator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider c) {
		if (c.name != "w1") {
			doorAnimator.Play ("doorOpen");
		}
	}


}
