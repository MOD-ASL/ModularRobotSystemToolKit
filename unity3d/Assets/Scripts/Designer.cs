using UnityEngine;
using System.Collections;

public class Designer : MonoBehaviour {

	GameObject selectedModule;
	UIManager UIManagerScript;


	// Use this for initialization
	void Start () {
		UIManagerScript = gameObject.GetComponent<UIManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			SelectModule ();
		}
	}

	void SelectModule () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, 100))
		{
			GameObject parent = hit.transform.parent.gameObject;
			if (parent.tag == "Module") {
				selectedModule = parent;
				UIManagerScript.SetSelectedModule (selectedModule);
			}
		}
	}
}
