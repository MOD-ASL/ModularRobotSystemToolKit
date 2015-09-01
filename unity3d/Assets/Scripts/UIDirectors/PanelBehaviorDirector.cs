using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelBehaviorDirector : MonoBehaviour {

    public UI2MaComDirector uI2MaComDirector;

    public GameObject buttonRobotStateObject;

    public ScrollRect scrollRect;
    public Toggle toggleAutoLoop;

    public Transform panelBehaviorList;

    [HideInInspector]
    public ButtonRobotStateObjectDirector currentButtonRobotStateObjectDirector;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnDrag () {
        transform.position = Input.mousePosition;
    }
    
    public void ShowPanelOrNot (bool show) {
        gameObject.SetActive (show);
    }

    public GameObject AddButton (RobotStateObject rso) {
        GameObject newButton = Instantiate (buttonRobotStateObject) as GameObject;
        ButtonRobotStateObjectDirector buttonRobotStateObjectDirector = newButton.GetComponent <ButtonRobotStateObjectDirector> ();
        buttonRobotStateObjectDirector.nameLabel.text = "State: \n" + rso.name;
        buttonRobotStateObjectDirector.robotStateObject = rso;
        buttonRobotStateObjectDirector.button.onClick.AddListener ( () => OnButtonClick (buttonRobotStateObjectDirector));
        newButton.transform.SetParent (panelBehaviorList);
        scrollRect.normalizedPosition = new Vector2 (0f, 0f);
        return newButton;
    }

    public void DeleteButton () {
        if (currentButtonRobotStateObjectDirector == null) {
            uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot delete state: No state is selected.");
        }
        else {
            Destroy (currentButtonRobotStateObjectDirector.button.gameObject);
            currentButtonRobotStateObjectDirector = null;
        }
    }

    public GameObject InsertButton (int index, RobotStateObject rso) {
        GameObject button = AddButton (rso);
        button.transform.SetSiblingIndex (index);
        return button;
    }

    public GameObject ChangeButton (int index, RobotStateObject rso) {
        GameObject button = AddButton (rso);
        button.transform.SetSiblingIndex (index);
        DeleteButton ();
        return button;
    }
    
    public void ClearAllButtons () {
        List<GameObject> children = new List<GameObject> ();
        foreach (Transform child in panelBehaviorList.transform) children.Add (child.gameObject);
        children.ForEach (child => Destroy (child));
        currentButtonRobotStateObjectDirector = null;
    }

    public void OnToggleAutoLoop () {
        if (toggleAutoLoop.isOn) {
            if (currentButtonRobotStateObjectDirector != null) {
                currentButtonRobotStateObjectDirector.SetSeletedOrNot (false);
            }
            uI2MaComDirector.ma2UIComManager.ma2MaComManager.behaviorManager.robotStateObjectIndex = 0;
        }
    }

    public void OnButtonClick (ButtonRobotStateObjectDirector buttonRobotStateObjectDirector) {
        if (currentButtonRobotStateObjectDirector != null) {
            currentButtonRobotStateObjectDirector.SetSeletedOrNot (false);
        }
        currentButtonRobotStateObjectDirector = buttonRobotStateObjectDirector;
        uI2MaComDirector.ma2UIComManager.ma2MaComManager.behaviorManager.SetCurrentRobotStateObject (currentButtonRobotStateObjectDirector.robotStateObject);
        currentButtonRobotStateObjectDirector.SetSeletedOrNot (true);
        uI2MaComDirector.ma2UIComManager.ma2MaComManager.robotManager.SetRobotState (currentButtonRobotStateObjectDirector.robotStateObject);
    }
}

