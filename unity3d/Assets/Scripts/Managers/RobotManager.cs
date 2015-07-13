using UnityEngine;
using System.Collections;

public class RobotManager : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;
    public Transform robot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public RobotStateObject GetRobotStateObject () {
        RobotStateObject rso = new RobotStateObject ();
        rso.listOfModuleStateObjects = ma2MaComManager.modulesManager.GetAllModuleStateObjects ();
        return rso;
    }

    public void SetRobotState (RobotStateObject rso) {
        ma2MaComManager.modulesManager.SetAllModuleStateObjects (rso.listOfModuleStateObjects);
    }

}
