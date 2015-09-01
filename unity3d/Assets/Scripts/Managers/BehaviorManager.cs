using UnityEngine;
using System;
using System.Collections;

public class BehaviorManager : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;
    [HideInInspector]
    public BehaviorObject currentBehaviorObject;
    [HideInInspector]
    public RobotStateObject currentRobotStateObject = null;
    [HideInInspector]
    public IEnumerator currentCoroutine;

    private bool reachRobotStateObject = true;
    public int robotStateObjectIndex = 0;
    public bool play = false;

	// Use this for initialization
	void Start () {
        currentBehaviorObject = new BehaviorObject ();
	}
	
	// Update is called once per frame
	void Update () {
        if (play) {
            Play ();
        }
	}

    public void OnClickAddState () {
        RobotStateObject rso = ma2MaComManager.robotManager.GetRobotStateObject ();
        rso.name = Guid.NewGuid ().ToString ();
        currentBehaviorObject.listOfRobotStateObjects.Add (rso);
        GameObject button = ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.AddButton (rso);
        rso.button = button;
    }

    public void OnClickDeleteState () {
        currentBehaviorObject.listOfRobotStateObjects.Remove (currentRobotStateObject);
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.DeleteButton ();
        currentRobotStateObject = null;
    }

    public void OnClickOverwriteState () {
        int index = currentBehaviorObject.listOfRobotStateObjects.IndexOf (currentRobotStateObject);
        if (index == -1) {
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot overwrite state: No state is selected.");
        }
        else {
            RobotStateObject rso = ma2MaComManager.robotManager.GetRobotStateObject ();
            rso.name = Guid.NewGuid ().ToString ();
            currentBehaviorObject.listOfRobotStateObjects[index] = rso;
            GameObject button = ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.ChangeButton (index, rso);
            rso.button = button;
            currentRobotStateObject = null;
        }
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
            GameObject button = ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.InsertButton (index, rso);
            rso.button = button;
        }
    }

    public void OnClickClear () {
		if (currentBehaviorObject != null) {
			currentBehaviorObject.listOfRobotStateObjects.Clear ();
		}
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.ClearAllButtons ();
        currentRobotStateObject = null;
    }

    public void Play () {
        if (reachRobotStateObject && ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.toggleAutoLoop.isOn) {
            if (currentBehaviorObject.listOfRobotStateObjects.Count > 0) {
                currentBehaviorObject.listOfRobotStateObjects[robotStateObjectIndex].button.GetComponent<ButtonRobotStateObjectDirector> ().SetSeletedOrNot (true);
                if (currentCoroutine != null) {
                    StopCoroutine (currentCoroutine);
                }
                currentCoroutine = PlayRobotStateObjectAndWait (currentBehaviorObject.listOfRobotStateObjects[robotStateObjectIndex]);
                StartCoroutine (currentCoroutine);
                reachRobotStateObject = false;
            }
        } 
    }

    public IEnumerator PlayRobotStateObjectAndWait (RobotStateObject rso) {
        ma2MaComManager.robotManager.SetRobotState (rso);
        yield return new WaitForSeconds (rso.period);
        if (rso.button != null) {
            rso.button.GetComponent<ButtonRobotStateObjectDirector> ().SetSeletedOrNot (false);
        }
        robotStateObjectIndex++;
        if (robotStateObjectIndex == currentBehaviorObject.listOfRobotStateObjects.Count) {
            robotStateObjectIndex = 0;
        }
        reachRobotStateObject = true;
    }

    public void SetCurrentRobotStateObject (RobotStateObject rso) {
        currentRobotStateObject = rso;
    }
}
