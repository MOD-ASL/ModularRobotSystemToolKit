using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class SliderControl : MonoBehaviour {

	public Slider slider;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateJointInfo (GameObject target) {
		slider.onValueChanged.RemoveAllListeners ();
		string jointName;
		UnityAction<float> targetFunction;

		if (slider.name == "SliderCenterJoint") {
			jointName = ModuleController.PartNames.Body.ToString ();
			targetFunction = target.GetComponent<ModuleController> ().UpdateCenterJointAngle;
		}
		else if (slider.name == "SliderLeftJoint") {
			jointName = ModuleController.PartNames.LeftWheel.ToString ();
			targetFunction = target.GetComponent<ModuleController> ().UpdateLeftJointAngle;
		}
		else if (slider.name == "SliderRightJoint") {
			jointName = ModuleController.PartNames.RightWheel.ToString ();
			targetFunction = target.GetComponent<ModuleController> ().UpdateRightJointAngle;
		}
		else {
			jointName = ModuleController.PartNames.FrontWheel.ToString ();
			targetFunction = target.GetComponent<ModuleController> ().UpdateFrontJointAngle;
		}
		slider.onValueChanged.AddListener (targetFunction);
		slider.value = target.GetComponent<ModuleController> ().GetJointValue (jointName);
	}

	public void UpdateSliderValue (string newValue) {
		slider.value = float.Parse(newValue);
	}

	public void Reset () {
		slider.onValueChanged.RemoveAllListeners ();
		slider.value = 0.0f;
	}
}
