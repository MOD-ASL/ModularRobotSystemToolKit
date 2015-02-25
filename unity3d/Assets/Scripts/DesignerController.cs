using UnityEngine;
using System.Collections;

public class DesignerController : MonoBehaviour {
	private bool addMode = false;
	private bool dragMode = false;
	private bool snapMode = true;
	private Transform candidateModule;
	private Vector3 newCandidateModulePos;
	//private Vector3 screenPoint;
	//private Vector3 offset;
	private float distCam2Origin;
	private Color originalColor;
	private int moduleCount = 1;
	private Transform selectedModule;

	public Transform SMORES;
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		CheckKeyPress();
		ActionBasedOnMode();
	}

	void ToggleAddMode () {
		addMode = !addMode;
		if (addMode) {
			OnRaiseAddMode();
		}
		else {
			OnDropAddMode();
		}
	}

	void ToggleDragMode () {
		dragMode = !dragMode;
	}

	void ToggleSnapMode () {
		snapMode = !snapMode;
	}

	bool FindSelectedModule () {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;
		
		if( Physics.Raycast( ray, out hit, 500 ) ) {
			if (hit.transform.parent && hit.transform.parent.name.StartsWith("Module")) {
				selectedModule = hit.transform.parent;
				return true;
			}
		}
		return false;
	}

	void OnRaiseAddMode () {
		distCam2Origin = Camera.main.transform.position.magnitude;
		newCandidateModulePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distCam2Origin));
		candidateModule = Instantiate(SMORES, ProcessPosition(newCandidateModulePos), Quaternion.identity) as Transform;
		candidateModule.name = "Module_x";
		foreach (Transform other in candidateModule) {
			originalColor = other.gameObject.renderer.material.color;
			other.gameObject.renderer.material.color = Color.green;
		}
		Screen.showCursor = false;
	}

	void OnDropAddMode () {
		Destroy(candidateModule.gameObject);
		Screen.showCursor = true;
	}

	void CheckKeyPress() {
		if (!dragMode && Input.GetKeyDown(KeyCode.A)) {
			ToggleAddMode();
		}
		if (!addMode && FindSelectedModule()) {
			if (Input.GetKeyDown(KeyCode.X)) {
				selectedModule.Rotate(new Vector3(90,0,0), Space.World);
			}
			if (Input.GetKeyDown(KeyCode.Y)) {
				selectedModule.Rotate(new Vector3(0,90,0), Space.World);
			}
			if (Input.GetKeyDown(KeyCode.Z)) {
				selectedModule.Rotate(new Vector3(0,0,90), Space.World);
			}
			if (Input.GetKeyDown(KeyCode.M)) {
				OnSelectModule(true);
				dragMode = true;
			}
		}
		if (dragMode && Input.GetKeyDown(KeyCode.Escape)) {
			Screen.showCursor = true;
			dragMode = false;
			OnSelectModule(false);
		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			ToggleSnapMode();
		}
	}

	void OnSelectModule (bool select) {
		if (select) {
			foreach (Transform other in selectedModule) {
				originalColor = other.gameObject.renderer.material.color;
				other.gameObject.renderer.material.color = Color.green;
			}
		} else {
			foreach (Transform other in selectedModule) {
				other.gameObject.renderer.material.color = originalColor;
			}
		}
	}

	void ActionBasedOnMode () {
		if (addMode) {
			distCam2Origin = Camera.main.transform.position.magnitude;
			newCandidateModulePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distCam2Origin));
			candidateModule.position = ProcessPosition(newCandidateModulePos);

			if (Input.GetKeyDown(KeyCode.X)) {
				candidateModule.Rotate(new Vector3(90,0,0), Space.World);
			}
			if (Input.GetKeyDown(KeyCode.Y)) {
				candidateModule.Rotate(new Vector3(0,90,0), Space.World);
			}
			if (Input.GetKeyDown(KeyCode.Z)) {
				candidateModule.Rotate(new Vector3(0,0,90), Space.World);
			}

			if (Input.GetMouseButtonDown(0)) {
				// Added the module permanently
				candidateModule.name = "Module_" + moduleCount;
				foreach (Transform other in candidateModule) {
					other.gameObject.renderer.material.color = originalColor;
				}
				candidateModule.SetParent(transform);
				addMode = false;
				Screen.showCursor = true;
				moduleCount++;
			}
		}

		else if (dragMode) {
			distCam2Origin = Camera.main.transform.position.magnitude;
			newCandidateModulePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distCam2Origin));
			selectedModule.position = ProcessPosition(newCandidateModulePos);
		}
	}

	Vector3 ProcessPosition (Vector3 pos) {
		if (!snapMode) {
			return pos;
		}
		Vector3 newPos = new Vector3(Mathf.Round(pos.x/10)*10, Mathf.Round(pos.y/10)*10, Mathf.Round(pos.z/10)*10);
		newPos.y = Mathf.Max(newPos.y, 5);
		return newPos;
	}
}
