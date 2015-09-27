using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonBehaviorController : MonoBehaviour {

    public Button button;
    public Text nameLabel;
    public Image icon;
    public RobotState rState;

    public void OnClickButton () {
        SendMessageUpwards ("OnClickRState", gameObject.GetComponent<ButtonBehaviorController> ());
    }
}
