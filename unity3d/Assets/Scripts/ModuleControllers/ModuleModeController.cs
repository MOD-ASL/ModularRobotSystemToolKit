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
    public ModuleMode currentModuleMode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void Awake () {
        mo2MaComController = gameObject.GetComponent<Mo2MaComController> ();
        OnChangeMode ();
    }

    // Set the mode and update the module accordingly
    public void SetMode (ModuleMode mode) {
        if (mode != currentModuleMode) {
            currentModuleMode = mode;
            OnChangeMode ();
        }
    }

    public void OnChangeMode () {
        if (currentModuleMode == ModuleMode.Static) {
            SetAnchorOrNot (true);
            SetGravity (false);
        }
        if (currentModuleMode == ModuleMode.Simulation) {
            SetKinematic (false);
            SetGravity (true);
            SetTrigger (false);
        }
        if (currentModuleMode == ModuleMode.Edit) {
            SetGravity (false);
			SetTrigger (false);
            SetKinematic (false);
        }
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

    // set all parts to trigger or not
    public void SetTrigger (bool trigger) {
        foreach (GameObject part in mo2MaComController.moduleRefPointerController.GetAllPartPointers ()) {
            part.GetComponent<MeshCollider> ().isTrigger = trigger;
        }
    }

    public void SetAnchorOrNot (bool anchor) {
        mo2MaComController.moduleRefPointerController.GetNodePointerByName 
            (ModuleRefPointerController.PartNames.BackPlate.ToString ())
            .GetComponent<Rigidbody> ().isKinematic = anchor;
    }
}
