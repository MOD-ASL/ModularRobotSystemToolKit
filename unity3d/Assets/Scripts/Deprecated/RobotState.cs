using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotState {
    /* A robot state contains a list of module states.
     */
    public List<ModuleState> listOfModuleStates = new List<ModuleState> ();
    public string Name { get; set; }
    public ButtonBehaviorController button;

    public void AddState (ModuleState mState) {
        listOfModuleStates.Add (mState);
    }
}
