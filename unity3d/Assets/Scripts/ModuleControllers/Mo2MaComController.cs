using UnityEngine;
using System.Collections;

public class Mo2MaComController : MonoBehaviour {

    [HideInInspector]
    public Ma2MoComManager ma2MoComManager;
    [HideInInspector]
    public ModuleRefPointerController moduleRefPointerController;
    [HideInInspector]
    public ModuleConnectionController moduleConnectionController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        ma2MoComManager = FindObjectOfType<Ma2MoComManager> ();
        moduleRefPointerController = gameObject.GetComponent<ModuleRefPointerController> ();
        moduleConnectionController = gameObject.GetComponent<ModuleConnectionController> ();
    }

    public void OnMouseClick () {
        ma2MoComManager.OnMouseClickOverModule (gameObject);
    }
}
