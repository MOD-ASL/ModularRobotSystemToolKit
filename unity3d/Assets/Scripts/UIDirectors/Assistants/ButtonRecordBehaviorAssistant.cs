using UnityEngine;
using System.Collections;

public class ButtonRecordBehaviorAssistant : MonoBehaviour {
    private ButtonDirector buttonDirector;
    public PanelBehaviorDirector panelBehaviorDirector;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickRecord () {
        
        if (!buttonDirector) {
            buttonDirector = gameObject.GetComponent<ButtonDirector> ();
        }
        
        // Check if it is in mode
        if (buttonDirector.mode.status) {
            panelBehaviorDirector.ShowPanelOrNot (true);
        }
        else {
            panelBehaviorDirector.ShowPanelOrNot (false);
        }
    }
}
