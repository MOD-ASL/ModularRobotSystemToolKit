using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelBottomMenuController : MonoBehaviour {
	bool isSimulate = false;
	bool isAddModule = false;
	public Button buttonAddModule;

	// Use this for initialization
	void Start () {
	
	}

	public void OnClickSimulate (float data) {
		isSimulate = !isSimulate;
		UpdateButtonColor ();
	}

	public void OnClickAddModule (float data) {
		isAddModule = !isAddModule;
		UpdateButtonColor ();
	}

	void UpdateButtonColor () {
		ColorBlock cb;
		if (isAddModule) {
			cb = buttonAddModule.colors;
			cb.normalColor =  new Color (1.0f, 0.5f, 0.5f);
			buttonAddModule.colors = cb;
		}
		else if (isSimulate) {
			buttonAddModule.GetComponent<Button> ().interactable = false;
		}
		else {
			cb = buttonAddModule.colors;
			cb.normalColor =  new Color (1.0f, 1.0f, 1.0f);
			buttonAddModule.colors = cb;
			buttonAddModule.GetComponent<Button> ().interactable = true;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
