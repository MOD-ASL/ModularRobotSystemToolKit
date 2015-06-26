using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {
    public GameObject panelMenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey (KeyCode.Escape)) {
            panelMenu.SetActive (true);
        }
	}

    public void OnClickResume () {
        panelMenu.SetActive (false);
    }

    public void OnClickLevels () {
        Application.LoadLevel ("SelectLevel");
    }
}
