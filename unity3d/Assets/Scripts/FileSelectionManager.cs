using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class File {
    public string name;
    public string userName;
    public string url;
    public Button.ButtonClickedEvent thingToDo;
    
    public File (string n, string user, string u, System.Action<File> callback) {
        name = n;
        userName = user;
        url = u;
        thingToDo = new Button.ButtonClickedEvent ();
        thingToDo.AddListener (() => {callback (this);});
    }
}

public class FileSelectionManager : MonoBehaviour {
    
    public GameObject sampleButton;
    public List<File> fileList;
    public ScrollRect scrollRect;

    public Transform contentPanel;
    public GameObject panelFileSelectionManager;
    
    void Start () {
        
    }
    
    void Update () {

    }

    public void ShowPanelOrNot (bool show) {
        panelFileSelectionManager.SetActive (show);
    }
    
    public void PopulateList () {
        foreach (File file in fileList) {
            ShowItem (file);
        }
    }
    
    public void AddItemToListAndShow (File file) {
        fileList.Add (file);
        ShowItem (file);
    }
    
    public void ShowItem (File file) {
        GameObject newButton = Instantiate (sampleButton) as GameObject;
        ButtonBehaviorController button = newButton.GetComponent <ButtonBehaviorController> ();
        button.nameLabel.text = "Name: " + file.name + "\n" + "User: " + file.userName;
        button.button.onClick = file.thingToDo;
        newButton.transform.SetParent (contentPanel);
        scrollRect.normalizedPosition = new Vector2 (0f, 0f);
    }
    
    public void ClearList () {
        fileList = new List<File> ();
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in contentPanel.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

}