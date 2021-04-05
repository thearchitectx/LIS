using UnityEngine;
using System.Collections;
 
public class JiggleBone : MonoBehaviour {
	// Target and dynamic positions
	Vector3 dynamicPos = new Vector3();
 
	// Bone settings
	public Vector3 boneAxis = new Vector3(0,0,1);
    public Vector3 upAxis = new Vector3(0,1,0);
	public float targetDistance = 2.0f;
	public float bGravity = 0;
 
	// Dynamics settings
	public Vector3 bStiffness = new Vector3(0.1f, 0.1f, 0.1f);
	public float bMass = 0.9f;
	public float bDamping = 0.75f;
	public float bDistanceLimit = 100;
 
	// Dynamics variables
	Vector3 force = new Vector3();
	Vector3 acc = new Vector3();
	public Vector3 vel = new Vector3();
	bool firstFrame = true;
 
	// Squash and stretch variables
	// public bool SquashAndStretch = true;
	// public float sideStretch = 0.15f;
	// public Vector3 frontStretch = new Vector3(0.2f, 0.2f, 0.2f);
 
	void LateUpdate(){
		#if UNITY_EDITOR
		// Reset the bone rotation so we can recalculate the upVector and forwardVector
		transform.rotation = new Quaternion();
		Vector3 forwardVector = transform.TransformDirection(new Vector3((boneAxis.x * targetDistance),(boneAxis.y * targetDistance),(boneAxis.z * targetDistance)));
		Vector3 upVector = transform.TransformDirection(upAxis);
		#endif
 
		// Calculate target position
		Vector3 targetPos = transform.position + transform.TransformDirection(new Vector3((boneAxis.x * targetDistance),(boneAxis.y * targetDistance),(boneAxis.z * targetDistance)));
		if (firstFrame)
		{
			firstFrame = false;
			dynamicPos = targetPos;
		}
 
		float timeFactor = Time.deltaTime * 100;
		// Calculate force, acceleration, and velocity per X, Y and Z
		force.x = (targetPos.x - dynamicPos.x) * bStiffness.x;
		acc.x = force.x / bMass;
		vel.x += acc.x * (1 - bDamping) * timeFactor;
 
		force.y = (targetPos.y - dynamicPos.y) * bStiffness.y;
		acc.y = (force.y / bMass) - (bGravity / 10);
		vel.y += acc.y * (1 - bDamping) * timeFactor;
 
		force.z = (targetPos.z - dynamicPos.z) * bStiffness.z;
		acc.z = force.z / bMass;
		vel.z += acc.z * (1 - bDamping) * timeFactor;
		
		// Update dynamic postion
		dynamicPos += (vel + force)  * timeFactor;
		dynamicPos = new Vector3(
			Mathf.Clamp(dynamicPos.x, targetPos.x-bDistanceLimit, targetPos.x+bDistanceLimit),
			Mathf.Clamp(dynamicPos.y, targetPos.y-bDistanceLimit, targetPos.y+bDistanceLimit),
			Mathf.Clamp(dynamicPos.z, targetPos.z-bDistanceLimit, targetPos.z+bDistanceLimit)
		);
		
		// Set bone rotation to look at dynamicPos
		transform.LookAt(dynamicPos, upAxis);
        // Look using y axis
		// transform.Rotate(new Vector3(90, 0, 0), Space.Self);
 
		// ==================================================
		// Green line is the bone's local up vector
		// Blue line is the bone's local foward vector
		// Yellow line is the target postion
		// Red line is the dynamic postion
		#if UNITY_EDITOR
		Debug.DrawRay(transform.position, forwardVector, Color.blue);
		Debug.DrawRay(transform.position, upVector, Color.green);
		Debug.DrawRay(targetPos, Vector3.up * 0.2f, Color.yellow);
		Debug.DrawRay(dynamicPos, Vector3.up * 0.2f, Color.red);
		#endif
		// ==================================================
	}
}