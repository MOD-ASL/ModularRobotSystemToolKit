using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class LevelList : MonoBehaviour {

    public GameObject sampleButton;
    public ScrollRect scrollRect;
    public Transform contentPanel;

    [HideInInspector]
    public List<Level> levelList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PopulateList () {
        foreach (Level l in levelList) {
            ShowButton (l);
        }
    }

    private void ShowButton (Level l) {
        GameObject newButton = Instantiate (sampleButton) as GameObject;
        ButtonBehaviorController button = newButton.GetComponent <ButtonBehaviorController> ();
        button.nameLabel.text = l.name;
        button.button.onClick = l.thingToDo;
        newButton.transform.SetParent (contentPanel);
        scrollRect.normalizedPosition = new Vector2 (0f, 0f);
    }
}
