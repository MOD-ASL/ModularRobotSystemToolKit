using UnityEngine;
using System.Collections;

public class DummyModuleController : MonoBehaviour {

    private Renderer rend;
    public Color color;

	// Use this for initialization
	void Start () {
        ChangeColor ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ChangeColor () {
        foreach (Transform part in transform) {
            rend = part.gameObject.GetComponent<Renderer> ();
            rend.material.EnableKeyword ("_EMISSION");
            rend.material.SetColor ("_EmissionColor", color);
        }
    }
}
