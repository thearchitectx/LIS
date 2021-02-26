using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineDollyAutoPing : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 0.05f;
    [SerializeField] private float m_SmoothTime = 0.2f;
    private float m_Speed = 0;
    private float m_Target = 1;
    private CinemachineVirtualCamera m_Cinemachine;
    private CinemachineTrackedDolly m_Dolly;
    
    void Start()
    {
        this.m_Cinemachine = GetComponent<CinemachineVirtualCamera>();
        this.m_Dolly = this.m_Cinemachine.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    void Update()
    {
        if (CinemachineCore.Instance.IsLive(m_Cinemachine)) {
            if (m_Dolly.m_PathPosition > 0.98f && m_Target == 1) 
            {
                m_Target = 0;
            }
            else if (m_Dolly.m_PathPosition < 0.02f && m_Target == 0)
            {
                m_Target = 1;
            }
            m_Dolly.m_PathPosition = Mathf.SmoothDamp(m_Dolly.m_PathPosition, m_Target, ref m_Speed, m_SmoothTime, m_MaxSpeed);
        }
    }
}
