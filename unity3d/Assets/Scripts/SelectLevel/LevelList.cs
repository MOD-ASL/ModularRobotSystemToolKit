using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class LevelList : MonoBehaviour {

    public GameObject sampleButton;
    public ScrollRect scrollRect;
    public Transform contentPanel;
    public ScrollRect scrollRectWorkspaces;
    public Transform contentPanelWorkspaces;
    public GameObject panelWorkspaces;

    [HideInInspector]
    public Dictionary<string, List<Level>> taskDictionary;

    [HideInInspector]
    public string selectedTask;

	// Use this for initialization
	void Start () {
        taskDictionary = new Dictionary<string, List<Level>> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PopulateList () {
        foreach (string taskName in taskDictionary.Keys) {
            ShowButton (taskName, taskDictionary[taskName]);
        }
    }

    private void ShowButton (Level l) {
        GameObject newButton = Instantiate (sampleButton) as GameObject;
        ButtonBehaviorController button = newButton.GetComponent <ButtonBehaviorController> ();
        button.nameLabel.text = l.name;
        button.button.onClick = l.thingToDo;
        button.button.onClick.AddListener (() => { panelWorkspaces.SetActive (false); });
        newButton.transform.SetParent (contentPanelWorkspaces);
        scrollRectWorkspaces.normalizedPosition = new Vector2 (0f, 0f);
    }

    private void ShowButton (string taskName, List<Level> levelList) {
        GameObject newButton = Instantiate (sampleButton) as GameObject;
        ButtonBehaviorController button = newButton.GetComponent <ButtonBehaviorController> ();
        button.nameLabel.text = taskName;
        button.button.onClick.AddListener (() => { ShowPanelWorkspaces (levelList); });
        button.button.onClick.AddListener (() => { selectedTask = taskName; });
        newButton.transform.SetParent (contentPanel);
        scrollRect.normalizedPosition = new Vector2 (0f, 0f);
    }

    public void ShowPanelWorkspaces (List<Level> levelList) {
        panelWorkspaces.SetActive (true);
        ClearAllButtons ();
        foreach (Level l in levelList) {
            ShowButton (l);
        }
    }

    private void ClearAllButtons () {
        List<GameObject> children = new List<GameObject> ();
        foreach (Transform child in contentPanelWorkspaces) children.Add (child.gameObject);
        children.ForEach (child => Destroy (child));
    }
}
