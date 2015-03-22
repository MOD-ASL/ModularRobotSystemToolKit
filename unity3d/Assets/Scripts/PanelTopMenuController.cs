using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelTopMenuController : MonoBehaviour {
	
	public Button buttonSimulate;
	public Button buttonClear;
	public Button buttonBringToGround;
	bool isSimulate = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickSimulate (float data) {
		isSimulate = !isSimulate;
		UpdateButtonColor ();
	}

	void UpdateButtonColor () {
		if (isSimulate) {
			ColorBlock cb = buttonSimulate.colors;
			cb.normalColor =  new Color (1.0f, 0.5f, 0.5f);
			buttonSimulate.colors = cb;
			buttonClear.GetComponent<Button> ().interactable = false;
			buttonBringToGround.GetComponent<Button> ().interactable = false;
		}
		else {
			ColorBlock cb = buttonSimulate.colors;
			cb.normalColor =  new Color (1.0f, 1.0f, 1.0f);
			buttonSimulate.colors = cb;
			buttonClear.GetComponent<Button> ().interactable = true;
			buttonBringToGround.GetComponent<Button> ().interactable = true;
		}
	}
}
