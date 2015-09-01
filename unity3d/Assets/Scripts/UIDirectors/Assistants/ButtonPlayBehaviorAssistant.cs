using UnityEngine;
using System.Collections;

public class ButtonPlayBehaviorAssistant : MonoBehaviour {

    public PanelBehaviorDirector panelBehaviorDirector;
    private ButtonDirector buttonDirector;
    public BehaviorManager behaviorManager;
    public UI2MaComDirector uI2MaComDirector;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickPlay () {
        
        if (!buttonDirector) {
            buttonDirector = gameObject.GetComponent<ButtonDirector> ();
        }
        
        // Check if it is in mode
        if (buttonDirector.mode.status) {
			uI2MaComDirector.ma2UIComManager.ma2MaComManager.robotManager.TakeSnapshot ();

            uI2MaComDirector.ma2UIComManager.ma2MaComManager.modulesManager.SetAllModuleMode (ModuleModeController.ModuleMode.Simulation);
			uI2MaComDirector.ma2UIComManager.ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (true);
            panelBehaviorDirector.ShowPanelOrNot (true);
            behaviorManager.robotStateObjectIndex = 0;
            behaviorManager.play = true;
        }
        else {
            behaviorManager.play = false;
            if (behaviorManager.currentCoroutine != null) {
                StopCoroutine (behaviorManager.currentCoroutine);
            }
            panelBehaviorDirector.ShowPanelOrNot (false);
			uI2MaComDirector.ma2UIComManager.ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (false);
            uI2MaComDirector.ma2UIComManager.ma2MaComManager.modulesManager.SetAllModuleMode (ModuleModeController.ModuleMode.Edit);
			uI2MaComDirector.ma2UIComManager.ma2MaComManager.robotManager.LiftRobot ();
        }
    }
}
