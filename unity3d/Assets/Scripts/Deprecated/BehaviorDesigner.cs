using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviorDesigner : MonoBehaviour {
    public GameObject robot;
    public CreateScrollList createBehaviorListScipt;

    int robotStateId = 0;
    int robotBehaviorId = 0;
    public RobotBehavior currentRBehavior;
    int targetRStateId = -1;
    bool reached;

	// Use this for initialization
	void Start () {
	    NewRobotBehavior ();
	}
	
	// Update is called once per frame
	void Update () {
	    if (targetRStateId > -1) {
            createBehaviorListScipt.OnClickRState (currentRBehavior.listOfRobotStates[targetRStateId].button);
            GoToRobotState (currentRBehavior.listOfRobotStates[targetRStateId]);
            if (reached) {
                targetRStateId++;
            }
            if (targetRStateId == currentRBehavior.listOfRobotStates.Count) {
                if (createBehaviorListScipt.toggleLoopPlay.isOn) {
                    targetRStateId = 0;
                }
                else {
                    targetRStateId = -1;
                }
            }
        }
        else {

        }
	}

    public void NewRobotBehavior () {
        currentRBehavior = new RobotBehavior ();
        currentRBehavior.Name = "RobotBehavior_" + robotBehaviorId.ToString ();
        robotBehaviorId++;
        robotStateId = 0;
        createBehaviorListScipt.ClearList ();
    }

    public void DeleteRobotState () {
        currentRBehavior.RemoveState (createBehaviorListScipt.currentButton.rState);
        createBehaviorListScipt.RemoveItem (new Item (createBehaviorListScipt.currentButton.rState));
    }

    // On click the Record button
    public void OnClickButtonRecord () {
        RobotState rState = new RobotState ();
        rState.Name = "RobotState_" + robotStateId.ToString ();

        foreach (Transform m in robot.transform) {
            ModuleState mState = new ModuleState (m.name);
            mState.RecordAngels (m.GetComponent<ModuleController> ().GetJointAnglesInArray ());
            rState.AddState (mState);
        }

        currentRBehavior.AddState (rState);
        createBehaviorListScipt.AddItemToListAndShow (new Item (rState));
        robotStateId++;
    }

    public void GoToRobotState (RobotState rState) {
        SetAllJointsDamper (10f);
        reached = true;
        foreach (ModuleState mState in rState.listOfModuleStates) {
            foreach (Transform child in robot.transform) {
                if (child.gameObject.name == mState.moduleName) {
                    // TODO: Should we compare name? Or uid?
                    reached = child.GetComponent<ModuleController> ().UpdateJointsFromModuleState (mState) && reached;
                    break;
                }
            }
        }
        SetAllJointsDamper (1f);
    }

    void SetAllJointsDamper (float mul) {
        foreach (Transform child in robot.transform) {
            child.GetComponent<ModuleController> ().jointDamperForce = 3000f*mul;
        }
    }

    public void PlayAllRobotStatesStartFromCurrent () {
        if (targetRStateId != -1) {
            return;
        }
        RobotState rState;
        if (createBehaviorListScipt.currentButton == null) {
            rState = currentRBehavior.listOfRobotStates[0];
        }
        else {
            rState = createBehaviorListScipt.currentButton.rState;
        }
        targetRStateId = currentRBehavior.listOfRobotStates.IndexOf (rState);
    }
}
