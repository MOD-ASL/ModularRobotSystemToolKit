using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonRobotStateObjectDirector : MonoBehaviour {
    public Button button;
    public Text nameLabel;
    public RobotStateObject robotStateObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetSeletedOrNot (bool selected) {
        if (selected) {
            button.image.color = button.colors.pressedColor;
        }
        else {
            button.image.color = button.colors.highlightedColor;
        }
    }
}
