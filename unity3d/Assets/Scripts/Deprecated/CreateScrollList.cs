using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item {
    public RobotState rState;
    public Button.ButtonClickedEvent thingToDo;

    public Item (RobotState r) {
        rState = r;
        thingToDo = new Button.ButtonClickedEvent ();
    }
}

public class CreateScrollList : MonoBehaviour {
    
    public GameObject sampleButton;
    public List<Item> itemList;
    public ScrollRect scrollRect;
    public Toggle toggleLoopPlay;
    
    public Transform contentPanel;

    public ButtonBehaviorController currentButton;
    public Button buttonDeleteRobotState;
    public Button buttonPlayBehavior;
    public BehaviorDesigner behaviorDesigner;
    public PanelModuleSettingController panelModuleSettingController;
    
    void Start () {

    }

    void Update () {
        buttonDeleteRobotState.GetComponent<Button> ().interactable = !(currentButton == null);
        buttonPlayBehavior.GetComponent<Button> ().interactable = !(behaviorDesigner.currentRBehavior.listOfRobotStates.Count == 0);
    }

    public void PopulateList () {
        foreach (Item item in itemList) {
            ShowItem (item);
        }
    }

    public void AddItemToListAndShow (Item item) {
        itemList.Add (item);
        ShowItem (item);
    }

    public void RemoveItem (Item item) {
        itemList.Remove (item);
        GameObject buttonToRemove = new GameObject ();
        foreach (Transform b in contentPanel.transform) {
            if (b.GetComponent<ButtonBehaviorController> ().rState == item.rState) {
                buttonToRemove = b.gameObject;
                break;
            }
        }
        Destroy (buttonToRemove);
        currentButton = null;
    }

    public void ShowItem (Item item) {
        GameObject newButton = Instantiate (sampleButton) as GameObject;
        ButtonBehaviorController button = newButton.GetComponent <ButtonBehaviorController> ();
        button.nameLabel.text = item.rState.Name;
        button.button.onClick = item.thingToDo;
        button.rState = item.rState;
        button.button.onClick.AddListener (button.OnClickButton);
        newButton.transform.SetParent (contentPanel);
        item.rState.button = button;
        scrollRect.normalizedPosition = new Vector2 (0f, 0f);
    }

    public void ClearList () {
        itemList = new List<Item> ();
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in contentPanel.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        currentButton = null;
    }

    public void OnClickRState (ButtonBehaviorController b) {
        ColorBlock cb;
        if (currentButton != null) {
            cb = currentButton.button.colors;
            cb.normalColor = new Color (1f, 1f, 1f);
            currentButton.button.colors = cb;
        }

        currentButton= b;

        cb = currentButton.button.colors;
        cb.normalColor = new Color (1f, 0.5f, 0.5f);
        currentButton.button.colors = cb;

        behaviorDesigner.GoToRobotState (currentButton.rState);
        panelModuleSettingController.UpdateSliderControlInfo ();
    }
}