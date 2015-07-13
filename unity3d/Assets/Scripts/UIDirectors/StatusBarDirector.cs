using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBarDirector : MonoBehaviour {

    public Text textStatus;
    public Text textMessage;

    private bool resetTimer = false;

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

    public void SetTempTextMessage (string s, float delay = 5.0f) {
        resetTimer = true;
        StartCoroutine (DisplayTextThenDeleteWithDelay (s, delay));
    }

    public IEnumerator DisplayTextThenDeleteWithDelay (string s, float delay) {
        SetTextMessage (s);
        yield return new WaitForSeconds (delay);
        if (resetTimer) {
            resetTimer = false;
            yield break;
        }
        ResetTextMessage ();
    }
}
