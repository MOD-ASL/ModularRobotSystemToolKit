using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotManager : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;
    public Transform robot;
    public GameObject newRobot;
    public Mode dragMode;
	public string currentConfigurationID;

    public GameObject nodeOnNewRobot;
	private RobotStateObject snapshotRSO;

	// Use this for initialization
	void Start () {
	    dragMode = ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("Drag");
        newRobot = new GameObject ("newRobot");
	}
	
	// Update is called once per frame
	void Update () {
        if (dragMode.status) {

            float distCam2Origin = Camera.main.transform.position.magnitude;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distCam2Origin));
            newRobot.transform.position = newPos;
            if (Input.GetMouseButtonDown (0)) {
                dragMode.status = false;
            }
            if (Input.GetKeyDown (KeyCode.R)) {
                newRobot.transform.Rotate (newRobot.transform.up, 90.0f, Space.World);
            }
            if (Input.GetKeyDown (KeyCode.Escape)) {
                dragMode.status = false;
                Destroy (newRobot);
            }
        }
	}

    public RobotStateObject GetRobotStateObject (bool withConnection = false, bool forConfiguration = false) {
        RobotStateObject rso = new RobotStateObject ();
        rso.listOfModuleStateObjects = ma2MaComManager.modulesManager.GetAllModuleStateObjects (forConfiguration);
        if (withConnection) {
            rso.listOfConnectionObjects = ma2MaComManager.connectionManager.GetAllConnectionObjects ();
        }
		rso.anchorModuleName = ma2MaComManager.modulesManager.anchorModule.name;
        foreach (ModuleStateObject mso in rso.listOfModuleStateObjects) {
            if (rso.period < mso.period) {
                rso.period = mso.period;
            }
        }
        return rso;
    }

	public void TakeSnapshot () {
		snapshotRSO = GetRobotStateObject (true);
	}

	public void ResetRobot () {
		SetRobotState (snapshotRSO, true);
	}

    public void SetRobotState (RobotStateObject rso, bool reset = false) {
        ma2MaComManager.modulesManager.SetAllModuleStateObjects (rso.listOfModuleStateObjects, reset);
    }

    public void SpawnRobot (RobotStateObject rso) {
        Destroy (newRobot);
        newRobot = new GameObject ("newRobot");
        ma2MaComManager.modulesManager.SpawnModules (rso.listOfModuleStateObjects, newRobot);
        StartCoroutine (ma2MaComManager.connectionManager.SpawnConnections (rso.listOfConnectionObjects, newRobot.transform));
        dragMode.status = true;
        nodeOnNewRobot = null;
    }

	public IEnumerator ReplaceRobot (RobotStateObject rso) {
		ma2MaComManager.modulesManager.Clear (false);
		yield return new WaitForSeconds (2f);

		ma2MaComManager.modulesManager.SpawnModules (rso.listOfModuleStateObjects);
		StartCoroutine (ma2MaComManager.connectionManager.SpawnConnections (rso.listOfConnectionObjects, robot));
		GameObject m = ma2MaComManager.modulesManager.FindModuleWithName (rso.anchorModuleName, robot);
		ma2MaComManager.modulesManager.SetAnchorModule (m);
		//ma2MaComManager.modulesManager.SetAllModuleMode (ModuleModeController.ModuleMode.Edit);

	}

    public void MoveNewRobot (GameObject n1, GameObject n2) {
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
        
        foreach (Transform module in newRobot.transform) {
            module.transform.position += posDiff;
        }
        
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in newRobot.transform) children.Add(child.gameObject);
        foreach(GameObject child in children) { 
            child.transform.SetParent (robot.transform);
            child.name = ma2MaComManager.modulesManager.FindNextAvailableName ();
            child.GetComponent<ModuleModeController> ().SetTrigger (false);
        }
    }

    // TODO: move this
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

}
