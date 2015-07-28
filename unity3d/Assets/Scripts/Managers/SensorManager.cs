using UnityEngine;
using System.Collections;

public class SensorManager : MonoBehaviour {

	public Ma2MaComManager ma2MaComManager;
	public Camera sensorCamera;
	public Light headLight;
	public Transform sensorCameraPrefab;
	public Transform headLightPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AttachSensorToSelectedModule () {
		if (sensorCamera == null) {
			sensorCamera = Instantiate (sensorCameraPrefab).GetComponent<Camera> ();
		}
		if (headLight == null) {
			headLight = Instantiate (headLightPrefab).GetComponent<Light> ();
		}
		if (ma2MaComManager.selectionManager.selectedModule) {
			if (sensorCamera.transform.parent == ma2MaComManager.selectionManager.selectedModule.
			    GetComponent<ModuleRefPointerController> ().
			    GetPartPointerByName (ModuleRefPointerController.PartNames.FrontWheel.ToString ()).transform) {
				sensorCamera.gameObject.SetActive (!sensorCamera.gameObject.activeSelf);
				headLight.gameObject.SetActive (sensorCamera.gameObject.activeSelf);
			}
			else {
				sensorCamera.gameObject.SetActive (true);
				headLight.gameObject.SetActive (true);
			}

			sensorCamera.transform.position = ma2MaComManager.selectionManager.selectedModule.
				GetComponent<ModuleRefPointerController> ().
					GetPartPointerByName (ModuleRefPointerController.PartNames.FrontWheel.ToString ()).transform.position;
			sensorCamera.transform.parent = ma2MaComManager.selectionManager.selectedModule.
				GetComponent<ModuleRefPointerController> ().
					GetPartPointerByName (ModuleRefPointerController.PartNames.FrontWheel.ToString ()).transform;
			headLight.transform.position = ma2MaComManager.selectionManager.selectedModule.
				GetComponent<ModuleRefPointerController> ().
					GetPartPointerByName (ModuleRefPointerController.PartNames.FrontWheel.ToString ()).transform.position;
			headLight.transform.parent = ma2MaComManager.selectionManager.selectedModule.
				GetComponent<ModuleRefPointerController> ().
					GetPartPointerByName (ModuleRefPointerController.PartNames.FrontWheel.ToString ()).transform;
		}
		else {
			sensorCamera.gameObject.SetActive (false);
			headLight.gameObject.SetActive (false);
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Please select a module before click \"Set Sensor\".");
		}
	}
}
