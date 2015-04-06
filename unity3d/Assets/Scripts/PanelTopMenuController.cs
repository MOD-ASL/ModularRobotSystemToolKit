using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelTopMenuController : MonoBehaviour {
	
	public Button buttonSimulate;
	public Button buttonClear;
	public Button buttonBringToGround;
	public Button buttonModuleSetting;
	public Button buttonSystem;
	bool isSystem = false;
	bool isSimulate = false;
	bool isAddModule = false;
	bool isConnectNode = false;
	ColorManager colorManager;

	// Use this for initialization
	void Start () {
		colorManager = (ColorManager) GameObject.FindObjectOfType<ColorManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickSimulate (float data) {
		isSimulate = !isSimulate;
		UpdateButtonColor ();
	}

	public void OnClickSystem (float data) {
		isSystem = !isSystem;
		UpdateButtonColor ();
	}

	public void OnClickAddModule (float data) {
		isAddModule = !isAddModule;
		UpdateButtonColor ();
	}

	public void OnClickConnectNode (float data) {
		isConnectNode = !isConnectNode;
		UpdateButtonColor ();
	}

	void UpdateButtonColor () {
		ColorBlock cb;
		if (isSimulate) {
			cb = buttonSimulate.colors;
			cb.normalColor =  colorManager.buttonInMode;
			buttonSimulate.colors = cb;
			buttonClear.GetComponent<Button> ().interactable = false;
			buttonBringToGround.GetComponent<Button> ().interactable = false;
			buttonModuleSetting.GetComponent<Button> ().interactable = false;
			buttonSystem.GetComponent<Button> ().interactable = false;
		}
		else if (isAddModule) {
			buttonSimulate.GetComponent<Button> ().interactable = false;
			buttonClear.GetComponent<Button> ().interactable = false;
			buttonBringToGround.GetComponent<Button> ().interactable = false;
			buttonModuleSetting.GetComponent<Button> ().interactable = false;
			buttonSystem.GetComponent<Button> ().interactable = false;
		}
		else if (isConnectNode) {
			buttonSimulate.GetComponent<Button> ().interactable = false;
			buttonClear.GetComponent<Button> ().interactable = false;
			buttonBringToGround.GetComponent<Button> ().interactable = false;
			buttonModuleSetting.GetComponent<Button> ().interactable = false;
			buttonSystem.GetComponent<Button> ().interactable = false;
		}
		else if (isSystem) {
			buttonSimulate.GetComponent<Button> ().interactable = false;
			buttonClear.GetComponent<Button> ().interactable = false;
			buttonBringToGround.GetComponent<Button> ().interactable = false;
			buttonModuleSetting.GetComponent<Button> ().interactable = false;
		}
		else {
			cb = buttonSimulate.colors;
			cb.normalColor =  colorManager.buttonNormal;
			buttonSimulate.colors = cb;
			buttonSimulate.GetComponent<Button> ().interactable = true;
			buttonClear.GetComponent<Button> ().interactable = true;
			buttonBringToGround.GetComponent<Button> ().interactable = true;
			buttonModuleSetting.GetComponent<Button> ().interactable = true;
			buttonSystem.GetComponent<Button> ().interactable = true;
		}
	}
}
