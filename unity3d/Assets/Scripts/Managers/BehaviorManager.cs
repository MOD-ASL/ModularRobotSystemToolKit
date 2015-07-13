using UnityEngine;
using System;
using System.Collections;

public class BehaviorManager : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;
    [HideInInspector]
    public BehaviorObject currentBehaviorObject;
    [HideInInspector]
    public RobotStateObject currentRobotStateObject = null;

	// Use this for initialization
	void Start () {
        currentBehaviorObject = new BehaviorObject ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickAddState () {
        RobotStateObject rso = ma2MaComManager.robotManager.GetRobotStateObject ();
        rso.name = Guid.NewGuid ().ToString ();
        currentBehaviorObject.listOfRobotStateObjects.Add (rso);
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.AddButton (rso);
    }

    public void OnClickDeleteState () {
        currentBehaviorObject.listOfRobotStateObjects.Remove (currentRobotStateObject);
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.DeleteButton ();
        currentRobotStateObject = null;
    }

    public void OnClickInsertState () {
        int index = currentBehaviorObject.listOfRobotStateObjects.IndexOf (currentRobotStateObject);
        if (index == -1) {
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot insert state: No state is selected.");
        }
        else {
            RobotStateObject rso = ma2MaComManager.robotManager.GetRobotStateObject ();
            rso.name = Guid.NewGuid ().ToString ();
            currentBehaviorObject.listOfRobotStateObjects.Insert (index, rso);
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.InsertButton (index, rso);
        }
    }

    public void OnClickClear () {
        currentBehaviorObject.listOfRobotStateObjects.Clear ();
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.ClearAllButtons ();
        currentRobotStateObject = null;
    }

    public void OnClickPlay () {
        ma2MaComManager.saveLoadManagerNew.Save (currentBehaviorObject);
    }

    public void SetCurrentRobotStateObject (RobotStateObject rso) {
        currentRobotStateObject = rso;
    }
}
