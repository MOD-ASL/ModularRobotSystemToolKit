using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelBottomMenuController : MonoBehaviour {
	bool isSimulate = false;
	bool isAddModule = false;
	bool isConnectNode = false;
	public Button buttonAddModule;
	public Button buttonDeleteModule;
	public Button buttonConnectNode;
	public Button buttonConnect;
	ColorManager colorManager;

	// Use this for initialization
	void Start () {
		colorManager = (ColorManager) GameObject.FindObjectOfType<ColorManager> ();
	}

	public void OnClickSimulate (float data) {
		isSimulate = !isSimulate;
		UpdateButtonColor ();
	}

	public void OnClickAddModule (float data) {
		isAddModule = !isAddModule;
		UpdateButtonColor ();
	}

	public void OnClickConnectNode (float data) {
		isConnectNode = !isConnectNode;
		buttonConnect.gameObject.SetActive (isConnectNode);
		UpdateButtonColor ();
	}

	void PopulateButtonList () {

	}

	void UpdateButtonColor () {
		ColorBlock cb;
		if (isAddModule) {
			cb = buttonAddModule.colors;
			cb.normalColor =  colorManager.buttonInMode;
			buttonAddModule.colors = cb;
			buttonConnectNode.GetComponent<Button> ().interactable = false;
			buttonDeleteModule.GetComponent<Button> ().interactable = false;
		}
		else if (isConnectNode) {
			cb = buttonConnectNode.colors;
			cb.normalColor =  colorManager.buttonInMode;
			buttonConnectNode.colors = cb;
			buttonAddModule.GetComponent<Button> ().interactable = false;
			buttonDeleteModule.GetComponent<Button> ().interactable = false;
		}
		else if (isSimulate) {
			buttonAddModule.GetComponent<Button> ().interactable = false;
			buttonConnectNode.GetComponent<Button> ().interactable = false;
			buttonDeleteModule.GetComponent<Button> ().interactable = false;
		}
		else {
			cb = buttonAddModule.colors;
			cb.normalColor =  colorManager.buttonNormal;
			buttonAddModule.colors = cb;
			cb = buttonConnectNode.colors;
			cb.normalColor =  colorManager.buttonNormal;
			buttonConnectNode.colors = cb;
			buttonAddModule.GetComponent<Button> ().interactable = true;
			buttonConnectNode.GetComponent<Button> ().interactable = true;
			buttonDeleteModule.GetComponent<Button> ().interactable = true;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
