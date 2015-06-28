using UnityEngine;
using System.Collections;

public class Ma2MoComManager : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseClickOverModule (GameObject module) {
        ma2MaComManager.selectionManager.SelectModule (module);
    }
}
