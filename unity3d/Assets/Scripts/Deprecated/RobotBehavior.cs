using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotBehavior {
    /* A robot behavior contains a list of robot states.
     */
    public List<RobotState> listOfRobotStates = new List<RobotState> ();
    public string Name { get; set; }

    public void AddState (RobotState rState) {
        listOfRobotStates.Add (rState);
    }

    public void RemoveState (RobotState rState) {
        listOfRobotStates.Remove (rState);
    }
}
