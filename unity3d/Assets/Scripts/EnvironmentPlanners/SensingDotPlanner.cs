using UnityEngine;
using System.Collections;

public class SensingDotPlanner : MonoBehaviour {

    private Camera sensorCamera;
    private Renderer rend;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (sensorCamera == null) {
            sensorCamera = FindSensorCamera ();
        }
        else {
            if (InSensorCameraView (sensorCamera)) {
                ChangeColor (Color.green);
            }
            else {
                ChangeColor (Color.red);
            }
        }
	}

    /// <summary>
    /// Return the camera object with given name
    /// </summary>
    /// <param name="name"> The name of the sensor camera. Default to ``SensorCamera''. </param>
    /// <returns> Camera object or null. </returns>
    Camera FindSensorCamera (string name="SensorCamera") {
        foreach (Camera c in FindObjectsOfType<Camera> ()) {
            if (c.name == name) {
                return c;
            }
        }
        return null;
    }

    /// <summary>
    /// Check whether the gameObject is in the given sensor camera's view.
    /// </summary>
    /// <param name="c"> The sensor camera of interest. </param>
    /// <returns></returns>
    bool InSensorCameraView (Camera c) {
        // First check whether an object's bounding box 
        // falls within the camera's view frustum.
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes (c);
        if (GeometryUtility.TestPlanesAABB (planes,
                                            gameObject.GetComponent<Collider> ().bounds)) {

            // Then cast a ray from the camera to see if the object is blocked by others
            Vector3 direction = transform.position - c.transform.position;
            RaycastHit hitInfo;
            // Ignore the modules
            // TODO: Should only ignore the sensor module, not all modules.
            int mask = 1 << 8;
            mask = ~mask;

            if (Physics.Raycast (c.transform.position,
                                 direction,
                                 out hitInfo,
                                 Mathf.Infinity,
                                 mask)) {
                if (hitInfo.collider.gameObject == gameObject) {
                    // The first object hit is the object itself
                    // No other objects block the view
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Change the color of the object with the given color.
    /// </summary>
    /// <param name="color"> The color change to. </param>
    void ChangeColor (Color color) {
        if (rend == null) {
            rend = gameObject.GetComponent<Renderer> ();
        }

        rend.material.color = color;
    }
}
