using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PanelFileSelectionDirector : MonoBehaviour {
    
    public GameObject sampleButton;
    public List<File> fileList;
    public ScrollRect scrollRect;
    
    public Transform panelFileList;
    public GameObject panelFileSelectionDirector;
    public UI2MaComDirector uI2MaComDirector;
    public SaveLoadManagerNew.FileType fileType;
    
    void Start () {
        
    }
    
    void Update () {
        
    }
    
    public void ShowPanelOrNot (bool show) {
        panelFileSelectionDirector.SetActive (show);
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
        newButton.transform.SetParent (panelFileList);
        scrollRect.normalizedPosition = new Vector2 (0f, 0f);
    }
    
    public void ClearList () {
        fileList = new List<File> ();
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in panelFileList.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }
    
}