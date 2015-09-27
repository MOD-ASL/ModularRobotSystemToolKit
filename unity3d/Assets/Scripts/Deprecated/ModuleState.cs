using UnityEngine;
using System.Collections;

public class ModuleState {
    /* A module state contains the joint angles of each DoF of a module.
     */

    public string moduleName;
    // Front, left, right, center angles.
    public float[] jointAngles = new float[4];

    // Constructor
    public ModuleState (string m) {
        moduleName = m;
    }

    public void RecordAngels (float[] newJointAngles) {
        jointAngles = newJointAngles;
    }

    public override string ToString () {
        return string.Format ("Module state for {0}: F:{1}, L:{2}, R:{3}, C:{4}", moduleName, jointAngles[0], jointAngles[1], jointAngles[2], jointAngles[3]);
    }
}
