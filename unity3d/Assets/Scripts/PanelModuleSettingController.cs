using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelModuleSettingController : MonoBehaviour {

	bool isShow = false;
	public Text selectedModuleName;
	public GameObject selectedModule;
	public Slider sliderCenterJoint;
	public Slider sliderLeftJoint;
	public Slider sliderRightJoint;
	public Slider sliderFrontJoint;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show (bool data) {
		isShow = !isShow;
		gameObject.SetActive (isShow);
	}

	void UpdateText () {
		if (selectedModule != null) {
			selectedModuleName.text = selectedModule.name;
		}
		else {
			selectedModuleName.text = "Not Selected";
		}
	}

	public void UpdateSelectedModule (GameObject module) {
		selectedModule = module;
		if (!isShow) {
			Show (true);
		}
		UpdateText ();
		UpdateSliderControlInfo ();
	}

	void UpdateSliderControlInfo () {
		sliderCenterJoint.GetComponent<SliderControl> ().UpdateJointInfo (selectedModule);
		sliderLeftJoint.GetComponent<SliderControl> ().UpdateJointInfo (selectedModule);
		sliderRightJoint.GetComponent<SliderControl> ().UpdateJointInfo (selectedModule);
		sliderFrontJoint.GetComponent<SliderControl> ().UpdateJointInfo (selectedModule);
	}
}
