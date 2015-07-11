using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Mode {
    public string name;
    public bool status;

    
    public Mode (string n) {
        name = n;
        status = false;
    }

    public void ToggleMode () {
        status = !status;
    }
}

public class ModeManagerNew : MonoBehaviour {

    private List<Mode> modeList = new List<Mode> ();

    public Ma2MaComManager ma2MaComManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public Mode AddNewMode (string name) {

        Mode m = new Mode (name);
        modeList.Add (m);
        return m;
    }

    public Mode GetOrCreateModeByName (string name) {
        foreach (Mode m in modeList) {
            if (m.name == name) {
                return m;
            }
        }
        return AddNewMode (name);
    }


    public void OnUpdateMode () {
        string newTextStatus = "Current Mode: ";
        foreach (Mode m in modeList) {
            if (m.status) {
                newTextStatus += (m.name + " ");
            }
        }
        if (newTextStatus == "Current Mode: ") {
            newTextStatus += "Normal";
        }
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTextStatus (newTextStatus);
    }

}
