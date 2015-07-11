using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelModuleSettingDirector : MonoBehaviour {

    public UI2MaComDirector uI2MaComDirector;

    public Text textModuleName;

    public PanelJointDirector panelJointDirectorFront;
    public PanelJointDirector panelJointDirectorLeft;
    public PanelJointDirector panelJointDirectorRight;
    public PanelJointDirector panelJointDirectorCenter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnDrag () {
        transform.position = Input.mousePosition;
    }

    public void ShowPanelOrNot (bool show) {
        gameObject.SetActive (show);
        if (show) {
            OnSelectModule ();
        }
    }

    private void UpdateModuleName (string name) {
        textModuleName.text = name;
    }

    private void UpdatePanelWithModel (GameObject module) {
        if (module != null) {
            UpdateModuleName (module.name);
            ModuleMotionController mmc = module.GetComponent<ModuleMotionController> ();
            panelJointDirectorFront.SetJointInfo (mmc, ModuleRefPointerController.PartNames.FrontWheel.ToString ());
            panelJointDirectorLeft.SetJointInfo (mmc, ModuleRefPointerController.PartNames.LeftWheel.ToString ());
            panelJointDirectorRight.SetJointInfo (mmc, ModuleRefPointerController.PartNames.RightWheel.ToString ());
            panelJointDirectorCenter.SetJointInfo (mmc, ModuleRefPointerController.PartNames.Body.ToString ());
        }
        else {
            ResetPanel ();
        }
    }

    private void ResetPanel () {
        UpdateModuleName ("Not Sssselected");
        panelJointDirectorFront.ResetJointInfo ();
        panelJointDirectorLeft.ResetJointInfo ();
        panelJointDirectorRight.ResetJointInfo ();
        panelJointDirectorCenter.ResetJointInfo ();
    }

    public void OnSelectModule () {
        UpdatePanelWithModel (uI2MaComDirector.ma2UIComManager.ma2MaComManager.selectionManager.selectedModule);
    }
}
