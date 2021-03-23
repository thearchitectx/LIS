using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class Walker : MonoBehaviour
{
    [SerializeField] private Transform m_Destination;
    [SerializeField] private float m_Speed = 1;
    [SerializeField] private float m_RotateSpeed = 600;
    private Animator m_Animator;
    private CharacterController m_CharacterController;

    void Start()
    {
        this.m_Animator = GetComponent<Animator>();
        this.m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = m_Destination.position - this.m_CharacterController.transform.position;

        if ( v.magnitude > 0.2f )
        {
            m_Animator.SetBool("walk", true);
            this.m_CharacterController.Move(v.normalized * Time.deltaTime * this.m_Speed);
            
            Quaternion rot = Quaternion.LookRotation(v, Vector3.up);
            this.m_CharacterController.transform.rotation = Quaternion.RotateTowards(
                this.m_CharacterController.transform.rotation,
                rot,
                Time.deltaTime * m_RotateSpeed
            );
            this.m_CharacterController.transform.rotation = Quaternion.Euler(0, this.m_CharacterController.transform.rotation.eulerAngles.y, 0);
        } else {
            m_Animator.SetBool("walk", false);
            this.m_CharacterController.transform.rotation = Quaternion.RotateTowards(this.m_CharacterController.transform.rotation,
                this.m_Destination.rotation,
                Time.deltaTime * m_RotateSpeed
            );
            this.m_CharacterController.transform.rotation = Quaternion.Euler(0, this.m_CharacterController.transform.rotation.eulerAngles.y, 0);
        }
    }
}
