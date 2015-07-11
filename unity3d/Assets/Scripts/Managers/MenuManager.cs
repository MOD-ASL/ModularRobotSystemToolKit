using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {
    public GameObject panelMenu;
    public Ma2MaComManager ma2MaComManager;

    private Mode mode;

	// Use this for initialization
	void Start () {
        mode = ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("Pause");
    }
    
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey (KeyCode.Escape)) {
            panelMenu.SetActive (true);
            mode.status = true;
            ma2MaComManager.modeManagerNew.OnUpdateMode ();
        }
	}

    public void OnClickResume () {
        panelMenu.SetActive (false);
        mode.status = false;
        ma2MaComManager.modeManagerNew.OnUpdateMode ();
    }

    public void OnClickLevels () {
        Application.LoadLevel ("SelectLevel");
    }
}
