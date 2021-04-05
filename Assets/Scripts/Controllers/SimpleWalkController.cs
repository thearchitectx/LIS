using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class SimpleWalkController : MonoBehaviour
{
    [SerializeField] private Transform m_Destination;
    [SerializeField] private float m_Speed = 1;
    [SerializeField] private float m_RotateSpeed = 600;
    [SerializeField] private string m_WalkParamName;
    private Animator m_Animator;
    private CharacterController m_CharacterController;

    public Transform Destination { get { return m_Destination; } set { m_Destination = value;} }

    void Start()
    {
        this.m_Animator = GetComponent<Animator>();
        this.m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Destination==null || Time.deltaTime==0)
            return;
            
        Vector3 v = m_Destination.position - this.m_CharacterController.transform.position;

        if ( v.magnitude > 0.2f )
        {
            if (!string.IsNullOrEmpty(m_WalkParamName))
                m_Animator.SetBool(m_WalkParamName, true);

            this.m_CharacterController.Move(v.normalized * Time.deltaTime * this.m_Speed);
            
            Quaternion rot = Quaternion.LookRotation(v, Vector3.up);
            this.m_CharacterController.transform.rotation = Quaternion.RotateTowards(
                this.m_CharacterController.transform.rotation,
                rot,
                Time.deltaTime * m_RotateSpeed
            );
            this.m_CharacterController.transform.rotation = Quaternion.Euler(0, this.m_CharacterController.transform.rotation.eulerAngles.y, 0);
        } else {
            if (!string.IsNullOrEmpty(m_WalkParamName))
                m_Animator.SetBool(m_WalkParamName, false);
            this.m_CharacterController.transform.rotation = Quaternion.RotateTowards(this.m_CharacterController.transform.rotation,
                this.m_Destination.rotation,
                Time.deltaTime * m_RotateSpeed
            );
            this.m_CharacterController.transform.rotation = Quaternion.Euler(0, this.m_CharacterController.transform.rotation.eulerAngles.y, 0);
        }
    }
}
