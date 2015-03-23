using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Designer : MonoBehaviour {

	GameObject selectedModule;
	UIManager UIManagerScript;
	GameObject robotState;
	GameObject ghostModules;
	public GameObject robot;
	bool isSimulate = false;
	bool isAddModule = false;
	public Transform modulePrefab; 
	Hashtable connectionTable = new Hashtable ();
	Hashtable ghostModuleToAvailableNodes = new Hashtable (); // ghost module to node
	List<GameObject> availableNodes;
	public Material defaultMaterial;
	GameObject moduleUnderMouse;
	string[] nodeNames = {"FrontWheel", "LeftWheel", "BackPlate", "RightWheel"}; 

	// Use this for initialization
	void Start () {
		UIManagerScript = gameObject.GetComponent<UIManager> ();
		robotState = new GameObject ();
		robotState.name = "RobotState";
		ghostModules = new GameObject ();
		ghostModules.name = "GhostModules";
		Clear ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isSimulate) {

		}
		else if (isAddModule) {
			if (moduleUnderMouse != null) moduleUnderMouse.GetComponent<ModuleController> ().OnHighlighted (false);
			moduleUnderMouse = FindModuleUnderMouse ();
			if (moduleUnderMouse != null) {
				moduleUnderMouse.GetComponent<ModuleController> ().OnHighlighted (true);
				if(Input.GetMouseButtonDown(0))
				{
					moduleUnderMouse.name = FindNextAvailableName ();
					moduleUnderMouse.transform.parent = robot.transform;
					moduleUnderMouse.tag = "Module";
					moduleUnderMouse.GetComponent<ModuleController> ().OnDeselected ();
					moduleUnderMouse.GetComponent<ModuleController> ().SetToTrigger (false);
					moduleUnderMouse.GetComponent<ModuleController> ().SetMode (1);

					GameObject node = FindConnectingNode (moduleUnderMouse, (GameObject) ghostModuleToAvailableNodes[moduleUnderMouse]);
					Connect (node, (GameObject) ghostModuleToAvailableNodes[moduleUnderMouse]);

					FindAvailableSpots ();
					PlotAvailableSpots ();

				}
				if (Input.GetKeyDown (KeyCode.R)) {
					moduleUnderMouse.transform.Rotate (moduleUnderMouse.transform.up, 90.0f, Space.World);
				}
			}
		}
		else {
			if(Input.GetMouseButtonDown(0))
				{
					SelectModule ();
				}
		}
	}

	GameObject FindConnectingNode (GameObject module, GameObject node) {
		int angle;
		string nodeName;
		if (node.name == "FrontWheel") {
			angle = Mathf.RoundToInt(module.transform.rotation.eulerAngles.y);
			int id = angle % 90;
			nodeName = nodeNames[(id + 2) % 4];
		}
		else if (node.name == "BackPlate") {
			angle = Mathf.RoundToInt(module.transform.rotation.eulerAngles.y);
			int id = angle % 90;
			nodeName = nodeNames[id];
		}
		else if (node.name == "LeftWheel") {
			angle = Mathf.RoundToInt(module.transform.rotation.eulerAngles.y);
			int id = angle % 90;
			nodeName = nodeNames[(id + 3) % 4];
		}
		else {
			angle = Mathf.RoundToInt(module.transform.rotation.eulerAngles.y);
			int id = angle % 90;
			nodeName = nodeNames[(id + 1) % 4];
		}

		foreach (GameObject n in module.GetComponent<ModuleController> ().GetAllNodes ()) {
			if (n.name == nodeName) {
				return n;
			}
		}
		return new GameObject ();
	}

	void SelectModule () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit, 100) && (hit.transform.parent != null))
		{
			GameObject parent = hit.transform.parent.gameObject;
			if (parent.tag == "Module") {
				if (selectedModule != null) selectedModule.GetComponent<ModuleController> ().OnDeselected ();
				selectedModule = parent;
				selectedModule.GetComponent<ModuleController> ().OnSelected ();
				UIManagerScript.SetSelectedModule (selectedModule);
			}
		}
	}

	GameObject FindModuleUnderMouse () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit, 100) && (hit.transform.parent != null))
		{
			GameObject parent = hit.transform.parent.gameObject;
			if (parent.tag == "Ghost") {
				return parent;
			}
		}

		return null;
	}
	


	public void BringToGround () {
		float minHeight = 10.0f;
		foreach (Transform child in robot.transform) {
			if (child.position.y < minHeight) minHeight = child.position.y;
		}
		foreach (Transform child in robot.transform) {
			child.transform.position += new Vector3 (0, - minHeight, 0);
		}
	}

	public void Clear () {
		RemoveAllModules ();
		selectedModule = null;
		connectionTable = new Hashtable ();
		UIManagerScript.SetSelectedModule (null);
		Transform clone = Instantiate (modulePrefab, new Vector3 (0, 3, 0), Quaternion.identity) as Transform;
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
			if (selectedModule != null) {
				selectedModule.GetComponent<ModuleController> ().OnDeselected ();
				selectedModule = null;
				UIManagerScript.SetSelectedModule (null);
			}
			foreach (Transform child in robot.transform) {
				Vector3 pos = child.GetComponent<ModuleController> ().backPlate.transform.position;
				Quaternion q = child.GetComponent<ModuleController> ().backPlate.transform.rotation;
				Transform clone = Instantiate (modulePrefab, pos, q) as Transform;
				clone.Rotate (clone.right, 90.0f, Space.World);
				clone.GetComponent<ModuleController> ().UpdateJointsFromModule (child.gameObject);
				clone.name = child.name;
				clone.SetParent (robotState.transform);
				clone.GetComponent<ModuleController> ().SetToTrigger (true);
				clone.GetComponent<ModuleController> ().SetMode (0);
				clone.GetComponent<ModuleController> ().OnLite (true);
				//clone.gameObject.SetActive (false);
			}

			foreach (Transform child in robot.transform) {
				child.GetComponent<ModuleController> ().SetToTrigger (false);
				child.GetComponent<ModuleController> ().SetMode (2);
			}
		}
		else {
			RemoveAllModules ();
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in robotState.transform) children.Add(child.gameObject);
			foreach (GameObject child in children) {
				if (child.name == "SMORES 0") {
					child.GetComponent<ModuleController> ().SetMode (0);
				}
				else {
					child.GetComponent<ModuleController> ().SetMode (1);
				}
				child.transform.SetParent (robot.transform);
				child.GetComponent<ModuleController> ().SetToTrigger (true);
				child.GetComponent<ModuleController> ().OnLite (false);
				child.SetActive (true);
			}

			foreach (string m1 in connectionTable.Keys) {
				string m2 = (string) connectionTable[m1];
				string[] stringSeparators = new string[] {":"};
				GameObject newM1 = new GameObject ();
				GameObject newM2 = new GameObject ();
				string[] m1Names = m1.Split (stringSeparators, StringSplitOptions.None);
				string[] m2Names = m2.Split (stringSeparators, StringSplitOptions.None);

				foreach (Transform child in robot.transform) {
					if (child.name == m1Names[0]) {
						foreach (Transform child2 in child.transform) {
							if (child2.name == m1Names[1]) {
								newM1 = child2.gameObject;
								break;
							}
						}
					}
					if (child.name == m2Names[0]) {
						foreach (Transform child2 in child.transform) {
							if (child2.name == m2Names[1]) {
								newM2 = child2.gameObject;
								break;
							}
						}
					}
				}

				FixedJoint j = newM1.AddComponent<FixedJoint> ();
				j.connectedBody = newM2.GetComponent<Rigidbody> ();
			}
		}
	
	}

	public void AddModule () {
		isAddModule = !isAddModule;
		if (isAddModule) {
			if (selectedModule != null) {
				selectedModule.GetComponent<ModuleController> ().OnDeselected ();
				selectedModule = null;
				UIManagerScript.SetSelectedModule (null);
			}
			FindAvailableSpots ();
			PlotAvailableSpots ();
		}
		else {
			Destroy (ghostModules);
			ghostModules = new GameObject ();
			ghostModules.name = "GhostModules";
		}
	}

	void InsertModuleAt (Vector3 position, GameObject connectedBody) {
		Transform clone = Instantiate (modulePrefab, position, Quaternion.identity) as Transform;
		clone.name = FindNextAvailableName ();
		clone.position = position;
		clone.parent = robot.transform;
		Connect (connectedBody, clone.GetComponent<ModuleController> ().backPlate);
	}

	string FindNextAvailableName () {
		int i = 1;
		while (GameObject.Find ("SMORES " + i.ToString ()) != null) {
			i++;
		}
		return "SMORES " + i.ToString ();
	}

	void Connect (GameObject m1, GameObject m2) {

		connectionTable.Add (m1.transform.parent.name+":"+m1.name, m2.transform.parent.name+":"+m2.name);
		connectionTable.Add (m2.transform.parent.name+":"+m2.name, m1.transform.parent.name+":"+m1.name);
		FixedJoint j = m1.AddComponent<FixedJoint> ();
		j.connectedBody = m2.GetComponent<Rigidbody> ();
	}

	void FindAvailableSpots () {
		availableNodes = new List<GameObject> ();
		GameObject node;

		foreach (Transform child in robot.transform) {
			node = child.GetComponent<ModuleController> ().frontWheel;
			if (!connectionTable.ContainsKey (child.name+":"+node.name)) {
				availableNodes.Add (node);
			}
			node = child.GetComponent<ModuleController> ().backPlate;
			if (!connectionTable.ContainsKey (child.name+":"+node.name)) {
				availableNodes.Add (node);
			}
			node = child.GetComponent<ModuleController> ().leftWheel;
			if (!connectionTable.ContainsKey (child.name+":"+node.name)) {
				availableNodes.Add (node);
			}
			node = child.GetComponent<ModuleController> ().rightWheel;
			if (!connectionTable.ContainsKey (child.name+":"+node.name)) {
				availableNodes.Add (node);
			}
		}
	}

	bool CheckOverlap (Vector3 pos) {
		foreach (Transform child in robot.transform) {
			if ((child.transform.position - pos).magnitude < 0.001f) {
				return true;
			}
		}
		foreach (Transform child in ghostModules.transform) {
			if ((child.transform.position - pos).magnitude < 0.001f) {
				return true;
			}
		}
		return false;
	}

	void PlotAvailableSpots () {
		Destroy (ghostModules);
		ghostModules = new GameObject ();
		ghostModules.name = "GhostModules";

		Vector3 pos;
		Quaternion q;

		foreach (GameObject node in availableNodes) {
			if (node.name == "FrontWheel") {
				pos = node.transform.position + node.transform.right;
				q = node.transform.rotation;
			}
			else if (node.name == "BackPlate") {
				pos = node.transform.position - node.transform.right;
				q = node.transform.rotation;
			}
			else if (node.name == "LeftWheel") {
				pos = node.transform.position - node.transform.up;
				q = node.transform.rotation;
			}
			else {
				pos = node.transform.position + node.transform.up;
				q = node.transform.rotation;
			}

			if (!CheckOverlap (pos)) {
				Transform clone = Instantiate (modulePrefab, pos, q) as Transform;
				clone.name = node.name;
				clone.tag = "Ghost";
				clone.parent = ghostModules.transform;
				clone.GetComponent<ModuleController> ().SetToTrigger (true);
				clone.transform.Rotate (transform.right, 90.0f);
				clone.GetComponent<ModuleController> ().OnHighlighted (false);

				ghostModuleToAvailableNodes[clone.gameObject] = node;
			}

		}
	}
}
