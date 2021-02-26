using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TheArchitect.Core.Controllers.Cinemachine
{
    /**
        Adapats a box collider confiner based on the FOV of a cinemachine lens
    **/
    public class CinemachineConfinerFOV : MonoBehaviour
    {   
        #pragma warning disable 0649
        [SerializeField] [Range(0, 1)] private float m_NormalizedValue = 0;
        [SerializeField] private float m_MinFieldOfView = 5;
        [SerializeField] private float m_MaxFieldOfView = 22;
        [SerializeField] private float m_MinConfinerWidth = 11;
        [SerializeField] private float m_MaxConfinerWidth = 17.5f;
        [SerializeField] private float m_MinConfinerHeight = 11;
        [SerializeField] private float m_MaxConfinerHeight = 17.5f;
        [SerializeField] private BoxCollider m_ConfinerCollider = null;
        #pragma warning restore 0649

        private CinemachineVirtualCamera m_VirtualCamera;

        public float Value {
            get { return this.m_NormalizedValue; }
            set { this.m_NormalizedValue = Mathf.Clamp01(value); }
        }

        // Start is called before the first frame update
        void Start()
        {
            m_VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        // Update is called once per frame
        void Update()
        {
            this.m_VirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(this.m_MaxFieldOfView, m_MinFieldOfView, this.m_NormalizedValue);

            this.m_ConfinerCollider.size = new Vector3(
                Mathf.Lerp(this.m_MinConfinerWidth, this.m_MaxConfinerWidth, this.m_NormalizedValue),
                Mathf.Lerp(this.m_MinConfinerHeight, this.m_MaxConfinerHeight, this.m_NormalizedValue),
                this.m_ConfinerCollider.size.z
            );
        }
    }

}