using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Designer : MonoBehaviour {

	GameObject selectedModule;
	GameObject selectedNode1;
	GameObject selectedNode2;
	public UIManager UIManagerScript;
    public ModeManager modeManager;
    public BehaviorDesigner behaviorDesigner;
	GameObject robotSpaceHolder;
	GameObject ghostModules;
	public GameObject robot;
	bool isDrag = false;
	public Transform modulePrefab; 
	Hashtable connectionTable = new Hashtable ();
	Hashtable ghostModuleToAvailableNodes = new Hashtable (); // ghost module to node
	List<GameObject> availableNodes = new List<GameObject> ();
	public Material defaultMaterial;
	GameObject moduleUnderMouse;
	GameObject nodeUnderMouse;
	string[] nodeNames = {"FrontWheel", "LeftWheel", "BackPlate", "RightWheel"};
	ColorManager colorManager;
	SaveLoadManager saveLoadManagerScript;
	public Button buttonDeleteModule;
	public Button buttonConnect;
    public Button buttonRecord;
	public Text buttonConnectText;
	public Text confName;
	GameObject newConf;

	// Use this for initialization
	void Start () {
		UIManagerScript = gameObject.GetComponent<UIManager> ();
		saveLoadManagerScript = gameObject.GetComponent<SaveLoadManager> ();
		colorManager = (ColorManager) GameObject.FindObjectOfType<ColorManager> ();
		robotSpaceHolder = new GameObject ();
		robotSpaceHolder.name = "RobotSpaceHolder";
		ghostModules = new GameObject ();
		newConf = new GameObject ();
		ghostModules.name = "GhostModules";
		Clear ();
	}
	
	// Update is called once per frame
	void Update () {
		if (modeManager.IsSimulate) {
            GameObject empty = (GameObject) GameObject.Find("New Game Object");
            if (empty){
                GameObject.Destroy(empty);
            }
		}
		else if (modeManager.IsAddModule) {
			PlotAvailableNodes (true);
			nodeUnderMouse = FindNodeUnderMouse ();
			moduleUnderMouse = FindModuleUnderMouse ();

			if (nodeUnderMouse != null) {
				FindAvailableSpots ();
				PlotAvailableSpots ();
			}
			if (moduleUnderMouse != null) {
				Renderer rend = ((GameObject) ghostModuleToAvailableNodes[moduleUnderMouse]).GetComponent<Renderer> ();
				rend.material.color = colorManager.moduleSelected;

				if (Input.GetMouseButtonDown(0)) {
					moduleUnderMouse.name = FindNextAvailableName ();
					moduleUnderMouse.transform.parent = robot.transform;
					moduleUnderMouse.tag = "Module";
					moduleUnderMouse.GetComponent<ModuleController> ().OnSelected (false);
					moduleUnderMouse.GetComponent<ModuleController> ().SetToTrigger (false);
					moduleUnderMouse.GetComponent<ModuleController> ().SetMode (1);

					GameObject node = FindConnectingNode (moduleUnderMouse, (GameObject) ghostModuleToAvailableNodes[moduleUnderMouse]);
					Connect (node, (GameObject) ghostModuleToAvailableNodes[moduleUnderMouse]);
				}
				if (Input.GetKeyDown (KeyCode.R)) {
					moduleUnderMouse.transform.Rotate (moduleUnderMouse.transform.up, 90.0f, Space.World);
				}
			}
		}
        else if (modeManager.IsRecordBehavior) {
            if(Input.GetMouseButtonDown(0)) {
                SelectModule ();
            }
        }
		else if (modeManager.IsConnectNodes) {
			PlotAvailableNodes (true);
			nodeUnderMouse = FindNodeUnderMouse ();
			if (nodeUnderMouse != null) {
				Renderer rend = nodeUnderMouse.GetComponent<Renderer> ();
				rend.material.color = colorManager.moduleSelected;
				if(Input.GetMouseButtonDown(0)) {
					if (selectedNode1 == null) {
						selectedNode1 = nodeUnderMouse;
					}
					else if ((selectedNode2 == null) && (selectedNode1 != nodeUnderMouse)) {
						selectedNode2 = nodeUnderMouse;
					}
					else {
						selectedNode1 = nodeUnderMouse;
						selectedNode2 = null;
					}

					if (selectedNode2 != null) {
						if (!CheckDifferentConf (selectedNode1, selectedNode2)) {
							buttonConnect.GetComponent<Button> ().interactable = true;
							buttonConnectText.text = "Connect two conf";
						}
						else {
							if (CheckNodeAdjacency (selectedNode1, selectedNode2)) {
								string  name = selectedNode1.transform.parent.name+":"+selectedNode1.name;
								if (!connectionTable.ContainsKey (name)) {
									buttonConnect.GetComponent<Button> ().interactable = true;
									buttonConnectText.text = "Connect";
								}
								else {
									buttonConnect.GetComponent<Button> ().interactable = true;
									buttonConnectText.text = "Disconnect";
								}
							}
							else {
								buttonConnectText.text = "Not adjacent";
							}
						}
					}
					else {
						buttonConnect.GetComponent<Button> ().interactable = false;
						buttonConnectText.text = "Select 2nd node";
					}
				}
			}

			if (selectedNode1 != null) {
				Renderer rend = selectedNode1.GetComponent<Renderer> ();
				rend.material.color = colorManager.moduleSelected;
			}

			if (selectedNode2 != null) {
				Renderer rend = selectedNode2.GetComponent<Renderer> ();
				rend.material.color = colorManager.moduleSelected;
			}
		}
		else {
			if(Input.GetMouseButtonDown(0)) {
				SelectModule ();
			}
		}

		if (isDrag) {
			float distCam2Origin = Camera.main.transform.position.magnitude;
			Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distCam2Origin));
			newConf.transform.position = newPos;
			if (Input.GetMouseButtonDown (0)) {
				saveLoadManagerScript.ConnectAllModules ();
				foreach (string m1 in saveLoadManagerScript.connectionTable.Keys) {
					string m2 = (string) saveLoadManagerScript.connectionTable[m1];
					connectionTable.Add (m1, m2);
				}
				saveLoadManagerScript.newConfCount += 1;
				isDrag = false;
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				newConf.transform.Rotate (newConf.transform.up, 90.0f, Space.World);
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isDrag = false;
				Destroy (newConf);
				newConf = new GameObject ();
			}
		}
	}

	bool CheckDifferentConf (GameObject n1, GameObject n2) {
		return (n1.transform.parent.parent == n2.transform.parent.parent);
	}

	public void OnClickSave () {
		saveLoadManagerScript.Save (robot.transform, connectionTable, confName.text);
	}

	public void OnClickLoad () {
		//newConf = saveLoadManagerScript.Load ();

		//isDrag = true;
	}

    public void OnLoadConfig (GameObject conf) {
        newConf = conf;
        isDrag = true;
    }

	Vector3 GetNodePosition (GameObject node) {
		Vector3 pos;
		if (node.name == "FrontWheel") {
			pos = node.transform.position + node.transform.right*0.5f;
		}
		else if (node.name == "BackPlate") {
			pos = node.transform.position - node.transform.right*0.5f;
		}
		else if (node.name == "LeftWheel") {
			pos = node.transform.position - node.transform.up*0.5f;
		}
		else {
			pos = node.transform.position + node.transform.up*0.5f;
		}
		return pos;
	}

	bool CheckNodeAdjacency (GameObject n1, GameObject n2) {
		return ((GetNodePosition (n1) - GetNodePosition (n2)).magnitude < 0.01f);
	}

	GameObject FindConnectingNode (GameObject module, GameObject node) {
		int angle;
		string nodeName;

		angle = Mathf.RoundToInt(module.transform.rotation.eulerAngles.y - node.transform.parent.rotation.eulerAngles.y);
		int id = angle / 90;
		if (id < 0) id +=4;

		if (node.name == "FrontWheel") {
			nodeName = nodeNames[(id + 2) % 4];
		}
		else if (node.name == "BackPlate") {
			nodeName = nodeNames[id];
		}
		else if (node.name == "LeftWheel") {
			nodeName = nodeNames[(id + 3) % 4];
		}
		else {
			nodeName = nodeNames[(id + 1) % 4];
		}
		return module.GetComponent<ModuleController> ().partsHashTable[nodeName];
	}

	void SelectModule () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit, 100) && (hit.transform.parent != null))
		{
			GameObject parent = hit.transform.parent.gameObject;
			if (parent.tag == "Module") {
				if (selectedModule != null) selectedModule.GetComponent<ModuleController> ().OnSelected (false);
				selectedModule = parent;
				selectedModule.GetComponent<ModuleController> ().OnSelected (true);
				UIManagerScript.SetSelectedModule (selectedModule);
				if (parent.name == "SMORES_0") {
					buttonDeleteModule.GetComponent<Button> ().interactable = true;
				}
				else {
					buttonDeleteModule.GetComponent<Button> ().interactable = true;
				}
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

	GameObject FindNodeUnderMouse () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit, 100) && (hit.transform.parent != null))
		{
			if ((hit.transform.parent.tag == "Module") &&(hit.transform.tag == "Node")) {
				return hit.transform.gameObject;
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
		Transform clone = Instantiate (modulePrefab, new Vector3 (0, 5, 0), Quaternion.identity) as Transform;
		clone.name = "SMORES_0";
		clone.SetParent (robot.transform);
	}

	void RemoveAllModules () {
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in robot.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
	}

	public void Simulate () {
		if (modeManager.IsSimulate) {
			if (selectedModule != null) {
				selectedModule.GetComponent<ModuleController> ().OnSelected (false);
				selectedModule = null;
				UIManagerScript.SetSelectedModule (null);
			}
			foreach (Transform child in robot.transform) {
				Vector3 pos = child.GetComponent<ModuleController> ().partsHashTable[ModuleController.PartNames.BackPlate.ToString ()].transform.position;
				Quaternion q = child.GetComponent<ModuleController> ().partsHashTable[ModuleController.PartNames.BackPlate.ToString ()].transform.rotation;
				Transform clone = Instantiate (modulePrefab, pos, q) as Transform;
				clone.Rotate (clone.right, 90.0f, Space.World);
				clone.GetComponent<ModuleController> ().UpdateJointsFromModule (child.gameObject);
				clone.name = child.name;
				clone.SetParent (robotSpaceHolder.transform);
				clone.GetComponent<ModuleController> ().SetToTrigger (true);
				clone.GetComponent<ModuleController> ().SetMode (0);
				clone.GetComponent<ModuleController> ().OnLite (true);
				//clone.gameObject.SetActive (false);
			}

            robot.transform.position = new Vector3 (10, 0, 0);
			foreach (Transform child in robot.transform) {
				child.GetComponent<ModuleController> ().SetToTrigger (false);
				child.GetComponent<ModuleController> ().SetMode (2);
			}
		}
		else {
			RemoveAllModules ();
            robot.transform.position = new Vector3 (0, 0, 0);
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in robotSpaceHolder.transform) children.Add(child.gameObject);
			foreach (GameObject child in children) {
				if (child.name == "SMORES_0") {
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
		if (modeManager.IsAddModule) {
			if (selectedModule != null) {
				selectedModule.GetComponent<ModuleController> ().OnSelected (false);
				selectedModule = null;
				UIManagerScript.SetSelectedModule (null);
			}
			PlotAvailableNodes (true);
		}
		else {
			PlotAvailableNodes (false);
			Destroy (ghostModules);
			ghostModules = new GameObject ();
			ghostModules.name = "GhostModules";
		}
	}

	public void ConnectNodes () {
		if (modeManager.IsConnectNodes) {
			if (selectedModule != null) {
				selectedModule.GetComponent<ModuleController> ().OnSelected (false);
				selectedModule = null;
				UIManagerScript.SetSelectedModule (null);
			}
			buttonConnect.GetComponent<Button> ().interactable = false;
			buttonConnectText.text = "Select 1st node";
			PlotAvailableNodes (true);
		}
		else {
			selectedNode1 = null;
			selectedNode2 = null;
			PlotAvailableNodes (false);
		}
	}

	public void DeleteModule () {
		if (selectedModule != null) {
			foreach (GameObject node1 in selectedModule.GetComponent<ModuleController> ().nodesConnectionHashTable.Keys) {
				GameObject node2 = selectedModule.GetComponent<ModuleController> ().nodesConnectionHashTable[node1];
				if (node2 != null) {
					Disconnect (node1, node2, true);
				}
			}
			Destroy (selectedModule);
			selectedModule = null;
			UIManagerScript.SetSelectedModule (null);
		}
	}

	void InsertModuleAt (Vector3 position, GameObject connectedBody) {
		Transform clone = Instantiate (modulePrefab, position, Quaternion.identity) as Transform;
		clone.name = FindNextAvailableName ();
		clone.position = position;
		clone.parent = robot.transform;
		Connect (connectedBody, clone.GetComponent<ModuleController> ().partsHashTable[ModuleController.PartNames.BackPlate.ToString ()]);
	}

	string FindNextAvailableName () {
		int i = 1;
		while (GameObject.Find ("SMORES_" + i.ToString ()) != null) {
			i++;
		}
		return "SMORES_" + i.ToString ();
	}

	void Connect (GameObject m1, GameObject m2) {
		connectionTable.Add (m1.transform.parent.name+":"+m1.name, m2.transform.parent.name+":"+m2.name);
		connectionTable.Add (m2.transform.parent.name+":"+m2.name, m1.transform.parent.name+":"+m1.name);
		FixedJoint j = m1.AddComponent<FixedJoint> ();
		j.connectedBody = m2.GetComponent<Rigidbody> ();
		m1.transform.parent.GetComponent<ModuleController> ().OnConnectNode (m1, m2);
		m2.transform.parent.GetComponent<ModuleController> ().OnConnectNode (m2, m1);
	}

	void Disconnect (GameObject m1, GameObject m2, bool byDestroy = false) {
		connectionTable.Remove (m1.transform.parent.name+":"+m1.name);
		connectionTable.Remove (m2.transform.parent.name+":"+m2.name);
		Destroy (m1.GetComponent<FixedJoint> ());
		Destroy (m2.GetComponent<FixedJoint> ());
		if (!byDestroy) {
			m1.transform.parent.GetComponent<ModuleController> ().OnDisconnectNode (m1);
		}
		m2.transform.parent.GetComponent<ModuleController> ().OnDisconnectNode (m2);
	}

	public void OnClickConnect () {
		if (buttonConnectText.text == "Connect") {
			Connect (selectedNode1, selectedNode2);
			selectedNode1 = null;
			selectedNode2 = null;
		}
		else if (buttonConnectText.text == "Disconnect") {
			Disconnect (selectedNode1, selectedNode2);
			selectedNode1 = null;
			selectedNode2 = null;
		}
		else if (buttonConnectText.text == "Connect two conf") {
			MoveNewConf (selectedNode1, selectedNode2);
			Connect (selectedNode1, selectedNode2);
			selectedNode1 = null;
			selectedNode2 = null;
		}
		buttonConnect.GetComponent<Button> ().interactable = false;
		buttonConnectText.text = "Select 1st node";
	}

	void MoveNewConf (GameObject n1, GameObject n2) {
		GameObject baseNode;
		GameObject remoteNode;

		if (n1.transform.parent.parent.name != "Robot") {
			baseNode = n2;
			remoteNode = n1;
		}
		else {
			baseNode = n1;
			remoteNode = n2;
		}

		Vector3 posDiff = GetNodePosition (baseNode) - GetNodePosition (remoteNode);

		foreach (Transform module in newConf.transform) {
			module.transform.position += posDiff;
		}

		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in newConf.transform) children.Add(child.gameObject);
		foreach(GameObject child in children) { 
		    child.transform.SetParent (robot.transform);
			child.GetComponent<ModuleController> ().SetToTrigger (false);
		}
	}

	void FindAvailableSpots () {
		availableNodes = new List<GameObject> ();
		string  name = nodeUnderMouse.transform.parent.name+":"+nodeUnderMouse.name;
		if (!connectionTable.ContainsKey (name)) {
			availableNodes.Add (nodeUnderMouse);
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
				clone.transform.Rotate (Vector3.right, 90.0f);
				clone.GetComponent<ModuleController> ().OnHighlighted (false);

				ghostModuleToAvailableNodes[clone.gameObject] = node;
			}

		}
	}

	void PlotAvailableNodes (bool p) {
		foreach (Transform module in robot.transform) {
			module.GetComponent<ModuleController> ().OnShowConnection (p);
		}
        if (newConf == null) {
            return;
        }
		foreach (Transform module in newConf.transform) {
			module.GetComponent<ModuleController> ().OnShowConnection (p);
		}
	}
}
