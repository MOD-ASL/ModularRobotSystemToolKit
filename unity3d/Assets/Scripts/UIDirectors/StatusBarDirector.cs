using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBarDirector : MonoBehaviour {

    public Text textStatus;
    public Text textMessage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTextStatus (string s) {
        textStatus.text = s;
    }

    public void ResetTextStatus () {
        SetTextStatus ("");
    }

    public void SetTextMessage (string s) {
        textMessage.text = s;
    }

    public void ResetTextMessage () {
        SetTextMessage ("");
    }
}
