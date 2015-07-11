using UnityEngine;
using System.Collections;

public class ModuleMotionController : MonoBehaviour {

    [HideInInspector]
    public Mo2MaComController mo2MaComController;

    // constant
    float jointSpringForce = 100000.0f;
    float jointDamperForce = 3000.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        // Ignore collisions among different parts within the module
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider1 in colliders) {
            foreach (Collider collider2 in colliders) {
                if (collider1 != collider2) {
                    Physics.IgnoreCollision(collider1, collider2);
                }
            }
        }

        mo2MaComController = gameObject.GetComponent<Mo2MaComController> ();
    }

    // return the joint value of the given joint name
    public float GetJointValue (string jointName, bool targetValue = true) {
        if (targetValue) {
            return mo2MaComController.moduleRefPointerController.GetHingeJointPointerByName (jointName).spring.targetPosition;
        }
        return mo2MaComController.moduleRefPointerController.GetHingeJointPointerByName (jointName).angle;
    }

    // update joint target position with the given value
    public void UpdateJointAngle (float jointValue, string jointName) {
        mo2MaComController.moduleRefPointerController.GetHingeJointPointerByName (jointName).GetComponent<Rigidbody> ().WakeUp ();
        JointSpring spring = mo2MaComController.moduleRefPointerController.GetHingeJointPointerByName (jointName).spring;
        spring.spring = jointSpringForce;
        spring.damper = jointDamperForce;
        spring.targetPosition = jointValue;
        mo2MaComController.moduleRefPointerController.GetHingeJointPointerByName (jointName).spring = spring;
        mo2MaComController.moduleRefPointerController.GetHingeJointPointerByName (jointName).useSpring = true;
    }
}
