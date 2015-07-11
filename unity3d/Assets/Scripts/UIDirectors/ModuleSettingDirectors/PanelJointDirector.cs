using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelJointDirector : MonoBehaviour {

    private InputField inputFieldJointAngle;
    private Slider sliderJointAngle;
    private string jointName;
    private ModuleMotionController moduleMotionController;

    public float initialAngle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        inputFieldJointAngle = gameObject.GetComponentInChildren<InputField> ();
        sliderJointAngle = gameObject.GetComponentInChildren<Slider> ();

        SetJointAngle (initialAngle);
    }

    public void UpdateText (float newValue) {
        inputFieldJointAngle.text = newValue.ToString();
    }

    public void UpdateSliderValue (string newValue) {
        sliderJointAngle.value = float.Parse(newValue);
    }

    public void UpdateSliderValue (float newValue) {
        sliderJointAngle.value = newValue;
    }

    public void SetJointAngle (float newValue) {
        UpdateText (newValue);
        UpdateSliderValue (newValue);
    }

    public void SetJointInfo (ModuleMotionController mmc, string name) {
        moduleMotionController = mmc;
        jointName = name;
        SetJointAngle (mmc.GetJointValue (jointName));
    }

    public void ResetJointInfo () {
        moduleMotionController = null;
        jointName = null;
        SetJointAngle (initialAngle);
    }

    public void OnUserChangeJointAngle (float newValue) {
        moduleMotionController.UpdateJointAngle (newValue, jointName);
    }

    public void OnUserChangeJointAngle (string newValue) {
        moduleMotionController.UpdateJointAngle (float.Parse (newValue), jointName);
    }

}
