using UnityEngine;
using System.Collections;

public class DummyModuleController : MonoBehaviour {

    private Renderer rend;
    public Color normalColor;
    public Color collisionColor;
    public bool collision;

	// Use this for initialization
	void Start () {
        ChangeColor (normalColor);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ChangeColor (Color color) {
        foreach (Transform part in transform) {
            rend = part.gameObject.GetComponent<Renderer> ();
            rend.material.color = color;
        }
    }

    void Awake () {
        // Check if there is collision
        collision = false;
        StartCoroutine (CheckCollision ());
    }

    public IEnumerator CheckCollision () {
        yield return null;
        foreach (Transform part in transform) {
            if (part.GetComponent<DummyPartController> ().touchedNode != null) {
                ChangeColor (collisionColor);
                collision = true;
            }
        }
    }
}
