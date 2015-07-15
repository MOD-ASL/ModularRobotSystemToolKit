using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveMenuDirector : MonoBehaviour {

    public GameObject panelSaveMenu;
    public Text fileName;
    public Text userName;
    public UI2MaComDirector uI2MaComDirector;

    public SaveLoadManagerNew.FileType fileType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickCancel () {
        ShowPanelOrNot (false);
    }

    public void ShowPanelOrNot (bool show) {
        panelSaveMenu.SetActive (show);
    }

    public void OnClickSave () {
        if (fileName.text == "") {
            uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot save file: No file name given.");
            return;
        }
        if (userName.text == "") {
            uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot save file: No user name given.");
            return;
        }

        if (fileType == SaveLoadManagerNew.FileType.Configuration) {
            uI2MaComDirector.ma2UIComManager.ma2MaComManager.saveLoadManagerNew.SaveCurrentConfiguration (fileName.text, userName.text);
        }

        panelSaveMenu.SetActive (false);
    }
}
