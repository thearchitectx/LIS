using UnityEngine;
using System.Collections;
 
public class ElasticHandleBone : MonoBehaviour {
	// Bone settings
	public float TargetDistance = 2.0f;
	public Vector3 TargetDirection = new Vector3(0,0,1);
    public Vector3 UpDirection = new Vector3(0,1,0);
 
	// Dynamics settings
	public float Gravity = 0;
	public float Stiffness = 0.1f;
	public float Mass = 1f;
	public float Damping = 0.75f;
	public float HandleDistanceLimit = 1;

    public Renderer CalculateWhenVisible;

	public bool FreezeHandleLocalX = false;
	public bool FreezeHandleLocalY = false;
	public bool FreezeHandleLocalZ = false;
 
	public Vector3 m_HandlePos = new Vector3();
	private Vector3 m_Velocity = Vector3.zero;
	private bool m_FirstFrame = true;
 
	void LateUpdate(){
		if (Time.deltaTime == 0)
			return;

        if (CalculateWhenVisible != null && !CalculateWhenVisible.isVisible)
            return;
			
		// Reset the bone rotation so we can recalculate the upVector and forwardVector
		transform.rotation = new Quaternion();
		Vector3 upVector = transform.TransformDirection(UpDirection);
 
		// Calculate target position
		Vector3 targetPos =  transform.position + transform.TransformDirection(new Vector3((TargetDirection.x * TargetDistance),(TargetDirection.y * TargetDistance),(TargetDirection.z * TargetDistance)));

		if (m_FirstFrame)
		{
			m_FirstFrame = false;
			m_HandlePos = targetPos;
		}
 
		Vector3 force, acc;
		// Calculate force, acceleration, and velocity per X, Y and Z
		force.x = (targetPos.x - m_HandlePos.x) * Stiffness;
		acc.x = force.x / Mass;
		m_Velocity.x += acc.x / Time.deltaTime;
		m_Velocity.x = m_Velocity.x * (1 - Damping);
		m_HandlePos.x += m_Velocity.x * Time.deltaTime;

		if (m_HandlePos.x < targetPos.x-HandleDistanceLimit || m_HandlePos.x > targetPos.x+HandleDistanceLimit)
		{
			m_HandlePos.x = Mathf.Clamp(m_HandlePos.x, targetPos.x-HandleDistanceLimit, targetPos.x+HandleDistanceLimit);
			m_Velocity.x = 0;
		}

		force.y = (targetPos.y - m_HandlePos.y) * Stiffness;
		acc.y = force.y / Mass;
		m_Velocity.y += acc.y / Time.deltaTime - (Gravity / 10 / Time.deltaTime);
		m_Velocity.y = m_Velocity.y * (1 - Damping);
		m_HandlePos.y += m_Velocity.y * Time.deltaTime;

		if (m_HandlePos.y < targetPos.y-HandleDistanceLimit || m_HandlePos.y > targetPos.y+HandleDistanceLimit)
		{
			m_HandlePos.y = Mathf.Clamp(m_HandlePos.y, targetPos.y-HandleDistanceLimit, targetPos.y+HandleDistanceLimit);
			m_Velocity.y = 0;
		}

		force.z = (targetPos.z - m_HandlePos.z) * Stiffness;
		acc.z = force.z / Mass;
		m_Velocity.z += acc.z / Time.deltaTime;
		m_Velocity.z = m_Velocity.z * (1 - Damping);
		m_HandlePos.z += m_Velocity.z * Time.deltaTime;

		if (m_HandlePos.z < targetPos.z-HandleDistanceLimit || m_HandlePos.z > targetPos.z+HandleDistanceLimit)
		{
			m_HandlePos.z = Mathf.Clamp(m_HandlePos.z, targetPos.z-HandleDistanceLimit, targetPos.z+HandleDistanceLimit);
			m_Velocity.z = 0;
		}

        var constrainedHandle = this.transform.InverseTransformPoint(m_HandlePos);
        var t = this.transform.InverseTransformPoint(targetPos);
        constrainedHandle.x = FreezeHandleLocalX ? t.x : constrainedHandle.x;
        constrainedHandle.y = FreezeHandleLocalY ? t.y : constrainedHandle.y;
        constrainedHandle.z = FreezeHandleLocalZ ? t.z : constrainedHandle.z;
        constrainedHandle = this.transform.TransformPoint(constrainedHandle);
		
		#if UNITY_EDITOR
		Debug.DrawLine(transform.position, targetPos, Color.blue);
		Debug.DrawRay(transform.position, upVector, Color.green);
		Debug.DrawLine(targetPos - Vector3.up * 0.05f, targetPos - Vector3.down * 0.05f, Color.red);
		Debug.DrawLine(targetPos - Vector3.left * 0.05f, targetPos - Vector3.right * 0.05f, Color.red);
		Debug.DrawLine(targetPos - Vector3.forward * 0.05f, targetPos - Vector3.back * 0.05f, Color.red);
		Debug.DrawLine(m_HandlePos - Vector3.up * 0.05f, m_HandlePos - Vector3.down * 0.05f, Color.yellow);
		Debug.DrawLine(m_HandlePos - Vector3.left * 0.05f, m_HandlePos - Vector3.right * 0.05f, Color.yellow);
		Debug.DrawLine(m_HandlePos - Vector3.forward * 0.05f, m_HandlePos - Vector3.back * 0.05f, Color.yellow);
		Debug.DrawLine(constrainedHandle - Vector3.up * 0.05f, constrainedHandle - Vector3.down * 0.05f, Color.magenta);
		Debug.DrawLine(constrainedHandle - Vector3.left * 0.05f, constrainedHandle - Vector3.right * 0.05f, Color.magenta);
		Debug.DrawLine(constrainedHandle - Vector3.forward * 0.05f, constrainedHandle - Vector3.back * 0.05f, Color.magenta);
		#endif
        
		// Set bone rotation to look at dynamicPos
		transform.LookAt(constrainedHandle, upVector);
 
		// ==================================================
	}
}