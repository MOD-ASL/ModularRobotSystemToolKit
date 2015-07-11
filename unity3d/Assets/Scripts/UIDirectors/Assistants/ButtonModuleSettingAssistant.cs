using UnityEngine;
using System.Collections;

public class ButtonModuleSettingAssistant : MonoBehaviour {

    private ButtonDirector buttonDirector;
    public PanelModuleSettingDirector panelModuleSettingDirector;

	// Use this for initialization
	void Start () {
	    buttonDirector = gameObject.GetComponent<ButtonDirector> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickButtonModuleSetting () {

        if (!buttonDirector) {
            buttonDirector = gameObject.GetComponent<ButtonDirector> ();
        }

        // Check if it is in mode
        if (buttonDirector.mode.status) {
            // Check if any module is selected
            if (buttonDirector.menuTopDirector.uI2MaComDirector.ma2UIComManager
                .ma2MaComManager.selectionManager.selectedModule != null) {
                panelModuleSettingDirector.ShowPanelOrNot (true);
                buttonDirector.menuTopDirector.uI2MaComDirector.statusBarDirector.ResetTextMessage ();
            }
            else {
                buttonDirector.menuTopDirector.uI2MaComDirector.statusBarDirector.SetTextMessage ("Please select a module ...");
            }
        }
        else {
            buttonDirector.menuTopDirector.uI2MaComDirector.statusBarDirector.ResetTextMessage ();
            panelModuleSettingDirector.ShowPanelOrNot (false);
        }
    }
}
