using UnityEngine;
using System.Collections;

public class JointCommandController : MonoBehaviour {

    public HingeJoint joint;

	public float motorForce = 1000f;

    private float targetValue = 0f;

    private float jointPositionCmdVelocity = 0f;
    private float jointTargetVelocity = 0f;

	public JointCommandObject.CommandTypes cmdType;

    // constant
    //float jointSpringForce = 100000.0f;
    float jointDamperForce = 30.0f;

	// Use this for initialization
	void Start () {
		cmdType = JointCommandObject.CommandTypes.Position;
	}
	
	// Update is called once per frame
	void Update () {
		if (cmdType == JointCommandObject.CommandTypes.Position) {
            float newValue = Mathf.SmoothDampAngle(joint.angle, targetValue, ref jointPositionCmdVelocity, 1f);
			SetJointAngle (newValue);
		}  
	}

	public void ChangeCommandType (JointCommandObject.CommandTypes type) {
		cmdType = type;
		if (cmdType == JointCommandObject.CommandTypes.Position) {
			joint.useMotor = false;
			joint.useSpring = true;
		}
		else if (cmdType == JointCommandObject.CommandTypes.Velocity) {
			joint.useMotor = true;
			joint.useSpring = false;
		}
	}

    // update joint target position with the given value
    public void UpdateJointAngle (float jointValue) {
        joint.GetComponent<Rigidbody> ().WakeUp ();
        targetValue = jointValue;
    }

    public void SetJointAngle (float jointValue) {
        JointSpring spring = joint.spring;
        //spring.spring = jointSpringForce;
        spring.damper = jointDamperForce;
        spring.targetPosition = jointValue;
        joint.spring = spring;
    }

	public void SetJointVelocity (float vel) {
		JointMotor motor = joint.motor;
		motor.force = motorForce;
		motor.targetVelocity = vel;
		joint.motor = motor;
	}

    public IEnumerator SetJointVelocityAndWait (float vel, float period) {
        JointMotor motor = joint.motor;
        motor.force = motorForce;
        motor.targetVelocity = vel;
        jointTargetVelocity = vel;
        joint.motor = motor;
        yield return new WaitForSeconds (period);
        motor.targetVelocity = 0.0f;
        joint.motor = motor;
    }

    public void StopJoint () {
        JointMotor motor = joint.motor;
        motor.targetVelocity = 0f;
        joint.motor = motor;
    }

    // return the joint value of the given joint name
    public float GetJointValue (bool targetValue = true) {
        if (targetValue) {
			if (cmdType == JointCommandObject.CommandTypes.Position) {
				return Mathf.Round(joint.spring.targetPosition * 10f) / 10f;
			}
			else if (cmdType == JointCommandObject.CommandTypes.Velocity) {
                return Mathf.Round(jointTargetVelocity * 10f) / 10f;
			}
        }
		if (cmdType == JointCommandObject.CommandTypes.Position) {
			return Mathf.Round(joint.angle * 10f) / 10f;
		}

		return Mathf.Round(joint.velocity * 10f) / 10f;;      
    }
}
