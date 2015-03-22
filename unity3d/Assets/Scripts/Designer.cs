using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Designer : MonoBehaviour {

	GameObject selectedModule;
	UIManager UIManagerScript;
	GameObject robotState;
	public GameObject robot;
	bool isSimulate = false;
	public Transform modulePrefab; 


	// Use this for initialization
	void Start () {
		UIManagerScript = gameObject.GetComponent<UIManager> ();
		robotState = new GameObject ();
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

	public void BringToGround () {
		float minHeight = 10.0f;
		foreach (Transform child in robot.transform) {
			if (child.position.y < minHeight) minHeight = child.position.y;
		}
		foreach (Transform child in robot.transform) {
			child.Translate (new Vector3 (0, - minHeight, 0));
		}
	}

	public void Clear () {
		RemoveAllModules ();
		UIManagerScript.SetSelectedModule (null);
		Transform clone = Instantiate (modulePrefab, new Vector3 (0, 5, 0), Quaternion.identity) as Transform;
		clone.name = "SMORES 0";
		clone.SetParent (robot.transform);
	}

	void RemoveAllModules () {
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in robot.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
	}

	public void Simulate () {
		isSimulate = !isSimulate;
		if (isSimulate) {
			foreach (Transform child in robot.transform) {
				Transform clone = Instantiate (modulePrefab, child.position, Quaternion.identity) as Transform;
				clone.name = child.name;
				clone.SetParent (robotState.transform);
				clone.GetComponent<ModuleController> ().UpdateJointsFromModule (child.gameObject);
				clone.gameObject.SetActive (false);
			}

			foreach (Transform child in robot.transform) {
				child.GetComponent<ModuleController> ().SetMode (2);
			}
		}
		else {
			RemoveAllModules ();
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in robotState.transform) children.Add(child.gameObject);
			foreach (GameObject child in children) {
				child.transform.SetParent (robot.transform);
				child.SetActive (true);
			}
		}
	}
}
