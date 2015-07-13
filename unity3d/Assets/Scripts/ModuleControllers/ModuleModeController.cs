using UnityEngine;
using System.Collections;

public class ModuleModeController : MonoBehaviour {

    [HideInInspector]
    public Mo2MaComController mo2MaComController;
    // Avaliable modes
    // Static mode: BackPlate is fixed in place, no gravity
    // Edit mode: No gravity
    // Simulate mode: Full dynamics with collision
    public enum ModuleMode {Static, Edit, Simulation};
    // currentMode of the module
    [HideInInspector]
    public int currentModuleMode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        mo2MaComController = gameObject.GetComponent<Mo2MaComController> ();
    }

    // Set the mode and update the module accordingly
    public void SetMode (int mode) {
        if (mode != currentModuleMode) {
            currentModuleMode = mode;
            OnChangeMode ();
        }
    }

    public void OnChangeMode () {

    }

    // set all parts to be kinematic or not
    public void SetKinematic (bool kinematic) {
        foreach (GameObject part in mo2MaComController.moduleRefPointerController.GetAllPartPointers ()) {
            part.GetComponent<Rigidbody> ().isKinematic = kinematic;
        }
    }
    
    // set all parts to use gravity or not
    public void SetGravity (bool gravity) {
        foreach (GameObject part in mo2MaComController.moduleRefPointerController.GetAllPartPointers ()) {
            part.GetComponent<Rigidbody> ().useGravity = gravity;
        }
    }

    public void SetAnchorOrNot (bool anchor) {
        mo2MaComController.moduleRefPointerController.GetNodePointerByName 
            (ModuleRefPointerController.PartNames.BackPlate.ToString ())
            .GetComponent<Rigidbody> ().isKinematic = anchor;
    }
}
