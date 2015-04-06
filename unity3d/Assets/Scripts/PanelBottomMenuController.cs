using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelBottomMenuController : MonoBehaviour {
	bool isSimulate = false;
	bool isAddModule = false;
	bool isConnectNode = false;
	bool isSystem = false;
	public Button buttonSystem;
	public Button buttonAddModule;
	public Button buttonDeleteModule;
	public Button buttonConnectNode;
	public Button buttonConnect;
	ColorManager colorManager;
	UIManager UIManagerScript;

	// Use this for initialization
	void Start () {
		UIManagerScript = (UIManager) GameObject.FindObjectOfType<UIManager> ();
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

	public void OnClickSystem (float data) {
		isSystem = !isSystem;
		UpdateButtonColor ();
	}

	public void OnClickConnectNode (float data) {
		isConnectNode = !isConnectNode;
		buttonConnect.gameObject.SetActive (isConnectNode);
		Vector2 size = gameObject.GetComponent<RectTransform> ().sizeDelta;
		size.y = (isConnectNode) ? 96.0f : 48.0f;
		gameObject.GetComponent<RectTransform> ().sizeDelta = size;
		UpdateButtonColor ();
	}

	void UpdateButtonColor () {
		ColorBlock cb;
		if (isAddModule) {
			cb = buttonAddModule.colors;
			cb.normalColor =  colorManager.buttonInMode;
			buttonAddModule.colors = cb;
			buttonConnectNode.GetComponent<Button> ().interactable = false;
			buttonDeleteModule.GetComponent<Button> ().interactable = false;
			buttonSystem.GetComponent<Button> ().interactable = false;
		}
		else if (isConnectNode) {
			cb = buttonConnectNode.colors;
			cb.normalColor =  colorManager.buttonInMode;
			buttonConnectNode.colors = cb;
			buttonAddModule.GetComponent<Button> ().interactable = false;
			buttonDeleteModule.GetComponent<Button> ().interactable = false;
			buttonSystem.GetComponent<Button> ().interactable = false;
		}
		else if (isSimulate) {
			buttonAddModule.GetComponent<Button> ().interactable = false;
			buttonConnectNode.GetComponent<Button> ().interactable = false;
			buttonDeleteModule.GetComponent<Button> ().interactable = false;
			buttonSystem.GetComponent<Button> ().interactable = false;
		}
		else if (isSystem) {
			cb = buttonSystem.colors;
			cb.normalColor =  colorManager.buttonInMode;
			buttonSystem.colors = cb;
			buttonAddModule.GetComponent<Button> ().interactable = false;
			buttonConnectNode.GetComponent<Button> ().interactable = false;
			buttonDeleteModule.GetComponent<Button> ().interactable = false;
			UIManagerScript.ShowSystemPanel (isSystem);
		}
		else {
			cb = buttonAddModule.colors;
			cb.normalColor =  colorManager.buttonNormal;
			buttonAddModule.colors = cb;
			cb = buttonConnectNode.colors;
			cb.normalColor =  colorManager.buttonNormal;
			buttonConnectNode.colors = cb;
			cb = buttonSystem.colors;
			cb.normalColor =  colorManager.buttonNormal;
			buttonSystem.colors = cb;
			buttonAddModule.GetComponent<Button> ().interactable = true;
			buttonConnectNode.GetComponent<Button> ().interactable = true;
			buttonDeleteModule.GetComponent<Button> ().interactable = true;
			buttonSystem.GetComponent<Button> ().interactable = true;
			UIManagerScript.ShowSystemPanel (isSystem);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
