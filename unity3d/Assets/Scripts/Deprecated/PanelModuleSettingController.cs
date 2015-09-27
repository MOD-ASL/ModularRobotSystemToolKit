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
    public Camera sensor;
    public Toggle toggleSensor;

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
        OnClickSensor (false);
        toggleSensor.isOn = false;
		selectedModule = module;
		UpdateText ();
		UpdateSliderControlInfo ();
	}

	public void UpdateSliderControlInfo () {
        if (selectedModule == null) {
            return;
        }
		sliderCenterJoint.GetComponent<SliderControl> ().UpdateJointInfo (selectedModule);
		sliderLeftJoint.GetComponent<SliderControl> ().UpdateJointInfo (selectedModule);
		sliderRightJoint.GetComponent<SliderControl> ().UpdateJointInfo (selectedModule);
		sliderFrontJoint.GetComponent<SliderControl> ().UpdateJointInfo (selectedModule);
	}

	public void Reset () {
        OnClickSensor (false);
        toggleSensor.isOn = false;
		selectedModule = null;
		isShow = false;
		gameObject.SetActive (isShow);
		UpdateText ();
		sliderCenterJoint.GetComponent<SliderControl> ().Reset ();
		sliderLeftJoint.GetComponent<SliderControl> ().Reset ();
		sliderRightJoint.GetComponent<SliderControl> ().Reset ();
		sliderFrontJoint.GetComponent<SliderControl> ().Reset ();
	}

    public void OnClickSensor (bool newValue) {
        if (selectedModule != null) {
            if (newValue) {
                selectedModule.GetComponent<ModuleController> ().AttachSensor (sensor);
            }
            else {
            selectedModule.GetComponent<ModuleController> ().ResetSensor ();
            }
        }
    }
}
