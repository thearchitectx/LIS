using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] public Transform m_Source;
    [SerializeField] private bool m_CopyWorldPosition;
    [SerializeField] private bool m_CopyWorldRotation;
    [SerializeField] private Vector3 m_PositionOffset;
    [SerializeField] private Vector3 m_RotationOffset;
    [SerializeField] public float TargetTransitionSpeed = 1f;

    private float m_TransitionProgress;
    private Transform m_LastSource;

    void Start()
    {
        this.m_LastSource = m_Source;
        this.m_TransitionProgress = 1;
    }

    void Update()
    {
        if (m_LastSource != m_Source && TargetTransitionSpeed > 0)
        {
            m_TransitionProgress = 0;
        }

        if (this.m_Source!=null)
        {
            if (this.m_CopyWorldPosition)
            {
                if (m_TransitionProgress < 1)
                {
                    m_TransitionProgress += (Time.deltaTime * TargetTransitionSpeed);

                    this.transform.position = Vector3.Lerp(
                        this.transform.position,
                        this.m_Source.transform.position + m_PositionOffset,
                        m_TransitionProgress
                    );
                }
                else
                {
                    this.transform.position = this.m_Source.transform.position + m_PositionOffset;
                }
            }
            if (this.m_CopyWorldRotation)
            {
                this.transform.rotation = this.m_Source.transform.rotation;
                this.transform.Rotate(m_RotationOffset);
            }
        }
    }
}
