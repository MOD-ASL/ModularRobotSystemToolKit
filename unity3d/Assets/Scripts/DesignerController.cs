using UnityEngine;
using System.Collections;

public class DesignerController : MonoBehaviour {
	private bool addMode = false;
	private bool dragMode = false;
	private Transform candidateModule;
	private Vector3 newCandidateModulePos;
	private Vector3 screenPoint;
	private Vector3 offset;
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

	Transform FindSelectedModule () {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;
		
		if( Physics.Raycast( ray, out hit, 500 ) ) {
			if (hit.transform.parent.name.StartsWith("Module")) {
				return hit.transform.parent;
			}
		}
		return null;
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
		if (!addMode && FindSelectedModule() != null) {
			if (Input.GetKeyDown(KeyCode.X)) {
				FindSelectedModule().Rotate(new Vector3(90,0,0), Space.World);
			}
			if (Input.GetKeyDown(KeyCode.Y)) {
				FindSelectedModule().Rotate(new Vector3(0,90,0), Space.World);
			}
			if (Input.GetKeyDown(KeyCode.Z)) {
				FindSelectedModule().Rotate(new Vector3(0,0,90), Space.World);
			}
			if (Input.GetKeyDown(KeyCode.M)) {
				selectedModule = FindSelectedModule();
				screenPoint = Camera.main.WorldToScreenPoint(selectedModule.position);
				offset = selectedModule.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				dragMode = true;
			}
		}
		if (dragMode && Input.GetKeyDown(KeyCode.Escape)) {
			Screen.showCursor = true;
			dragMode = false;
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
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			selectedModule.position = ProcessPosition(curPosition);
		}
	}

	Vector3 ProcessPosition (Vector3 pos) {
		if (!Input.GetKey(KeyCode.LeftShift)) {
			return pos;
		}
		Vector3 newPos = new Vector3(Mathf.Round(pos.x/10)*10, Mathf.Round(pos.y/10)*10, Mathf.Round(pos.z/10)*10);
		newPos.y = Mathf.Max(newPos.y, 5);
		return newPos;
	}
}
