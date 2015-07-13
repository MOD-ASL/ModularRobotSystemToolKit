using UnityEngine;
using System.Collections;

public class JointCommandController : MonoBehaviour {

    public HingeJoint joint;

    private float targetValue = 0f;

    private float jointVelocity = 0f;

    // constant
    //float jointSpringForce = 100000.0f;
    //float jointDamperForce = 3000.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float newValue = Mathf.SmoothDampAngle(joint.angle, targetValue, ref jointVelocity, 1f);
        SetJointAngle (newValue);
	}

    // update joint target position with the given value
    public void UpdateJointAngle (float jointValue) {
        joint.GetComponent<Rigidbody> ().WakeUp ();
        targetValue = jointValue;
    }

    public void SetJointAngle (float jointValue) {
        JointSpring spring = joint.spring;
        //spring.spring = jointSpringForce;
        //spring.damper = jointDamperForce;
        spring.targetPosition = jointValue;
        joint.spring = spring;
    }

    // return the joint value of the given joint name
    public float GetJointValue (bool targetValue = true) {
        if (targetValue) {
            return Mathf.Round(joint.spring.targetPosition * 10f) / 10f;
        }
        return Mathf.Round(joint.angle * 10f) / 10f;
    }
}
