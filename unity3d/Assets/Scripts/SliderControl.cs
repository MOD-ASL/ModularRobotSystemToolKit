using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class SliderControl : MonoBehaviour {

	Slider slider;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake () {
		slider = gameObject.GetComponent<Slider> ();
	}

	public void UpdateJointInfo (GameObject target) {
		slider.onValueChanged.RemoveAllListeners ();
		UnityAction<float> targetFunction;
		float jointValue;
		if (slider.name == "SliderCenterJoint") {
			targetFunction = target.GetComponent<ModuleController> ().UpdateCenterJointAngle;
			jointValue = target.GetComponent<ModuleController> ().GetJointValue ("centerJoint");
		}
		else if (slider.name == "SliderLeftJoint") {
			targetFunction = target.GetComponent<ModuleController> ().UpdateLeftJointAngle;
			jointValue = target.GetComponent<ModuleController> ().GetJointValue ("leftJoint");
		}
		else if (slider.name == "SliderRightJoint") {
			targetFunction = target.GetComponent<ModuleController> ().UpdateRightJointAngle;
			jointValue = target.GetComponent<ModuleController> ().GetJointValue ("rightJoint");
		}
		else {
			targetFunction = target.GetComponent<ModuleController> ().UpdateFrontJointAngle;
			jointValue = target.GetComponent<ModuleController> ().GetJointValue ("frontJoint");
		}
		slider.onValueChanged.AddListener (targetFunction);
		slider.value = jointValue;
	}
}
