using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectionManager : MonoBehaviour {

    public AdditionalCallback onSelectModuleCallback; //TODO: This is a hack

    [HideInInspector]
    public GameObject selectedModule;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SelectModule (GameObject module) {
        if ((selectedModule) && (selectedModule != module)) {
            selectedModule.GetComponent<ModuleInteractionController> ().SelectedOrNot (false);
        }
        selectedModule = module;
        selectedModule.GetComponent<ModuleInteractionController> ().SelectedOrNot (true);
        OnSelectModule ();
    }

    public void ResetSelectedModule () {
        if (selectedModule) {
            selectedModule.GetComponent<ModuleInteractionController> ().SelectedOrNot (false);
            selectedModule = null;
        }
    }

    public void OnSelectModule () {
        onSelectModuleCallback.Invoke ();
    }
}
