using UnityEngine;
using System.Collections;

public class Mo2MaComController : MonoBehaviour {

    [HideInInspector]
    public Ma2MoComManager ma2MoComManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        ma2MoComManager = FindObjectOfType<Ma2MoComManager> ();
    }

    public void OnMouseClick () {
        ma2MoComManager.OnMouseClickOverModule (gameObject);
    }
}
