using UnityEngine;
using System.Collections;

public class ModuleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
		foreach (Collider collider1 in colliders) {
			foreach (Collider collider2 in colliders) {
				if (collider1 != collider2) {
					Physics.IgnoreCollision(collider1, collider2);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
