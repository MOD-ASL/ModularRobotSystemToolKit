using UnityEngine;
using System.Collections;

public class GoalButtonPlanner : MonoBehaviour {

    public GameObject wall;
    public GameObject button;

	// Use this for initialization
	void Start () {
        if (GameObject.Find ("Stairs")) {
            button.transform.position += new Vector3 (0f, 1.5f, 0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider c) {
        Renderer rend = wall.GetComponent<Renderer> ();
        rend.material.color = Color.green;
	}


}
