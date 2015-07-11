using UnityEngine;
using System.Collections;

public class MenuTopDirector : MonoBehaviour {

    public UI2MaComDirector uI2MaComDirector;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SetAllButtonInteractableOrNot (bool interactable) {
        BroadcastMessage ("SetButtonInteractableOrNot", interactable);
    }

    public void OnUpdateMode () {
        uI2MaComDirector.ma2UIComManager.ma2MaComManager.modeManagerNew.OnUpdateMode ();
    }
}
