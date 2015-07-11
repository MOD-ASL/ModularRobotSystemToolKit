using UnityEngine;
using System.Collections;

public class DummyPartController : MonoBehaviour {
    [HideInInspector]
    public GameObject touchedNode;
    
    // Use this for initialization
    void Start () {
        touchedNode = null;
    }
    
    // Update is called once per frame
    void Update () {
        
    }
    
    public void OnTriggerEnter (Collider other) {
        if ((other.transform.parent.tag == "Module") && !(other is SphereCollider)) {
            touchedNode = other.gameObject;
        }
        
    }
    
    public void OnTriggerExit () {
        touchedNode = null;
    }
}
