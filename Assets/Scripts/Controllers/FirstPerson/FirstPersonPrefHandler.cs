using UnityEngine;
using Cinemachine;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Data.Variables;

namespace TheArchitect.Controllers.FirstPerson
{
    [RequireComponent(typeof(FirstPersonMouseLook), typeof(FirstPersonPlayerMovement))]
    public class FirstPersonPrefHandler : MonoBehaviour
    {
        public const float MIN_MOUSE_SENSITIVITY = 0.05f;
        public const float MAX_MOUSE_SENSITIVITY = 10;
        public const float DEFAULT_MOUSE_SENSITIVITY = 2;
        public const float MIN_FOV = 30;
        public const float MAX_FOV = 90;
        public const float DEFAULT_FOV = 55;
        public const string PREF_FOV = "FirstPerson.FOV";
        public const string PREF_MOUSE_SENSITIVITY = "FirstPerson.MOUSE_SENSITIVITY";

        private float m_PrefSaveCounter = 0;
        private CinemachineVirtualCamera m_VirtualCam;
        private FirstPersonMouseLook m_MouseLook;

        void Start()
        {
            this.m_MouseLook = GetComponentInChildren<FirstPersonMouseLook>();
            this.m_MouseLook.m_MouseSensitivity = PlayerPrefs.GetFloat(PREF_MOUSE_SENSITIVITY, DEFAULT_MOUSE_SENSITIVITY);
            this.m_VirtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
            this.m_VirtualCam.m_Lens.FieldOfView = PlayerPrefs.GetFloat(PREF_FOV, DEFAULT_FOV);

            if (this.m_MouseLook.m_MouseSensitivity >= MAX_MOUSE_SENSITIVITY || this.m_MouseLook.m_MouseSensitivity <= MIN_MOUSE_SENSITIVITY)
                this.m_MouseLook.m_MouseSensitivity = DEFAULT_MOUSE_SENSITIVITY;
        }

        void Update()
        {
            if (Time.deltaTime==0)
                return;

            if (Input.GetKey(KeyCode.F3)) {
                this.m_VirtualCam.m_Lens.FieldOfView = Mathf.Clamp(this.m_VirtualCam.m_Lens.FieldOfView - Time.deltaTime * 10, MIN_FOV, MAX_FOV);
                this.m_PrefSaveCounter = 0.5f;
            } else if (Input.GetKey(KeyCode.F4)) {
                this.m_VirtualCam.m_Lens.FieldOfView = Mathf.Clamp(this.m_VirtualCam.m_Lens.FieldOfView + Time.deltaTime * 10, MIN_FOV, MAX_FOV);
                this.m_PrefSaveCounter = 0.5f;
            } else if (Input.GetKey(KeyCode.F5)) {
                this.m_MouseLook.m_MouseSensitivity = Mathf.Clamp(this.m_MouseLook.m_MouseSensitivity - Time.deltaTime * 2, MIN_MOUSE_SENSITIVITY, MAX_MOUSE_SENSITIVITY);
                this.m_PrefSaveCounter = 0.5f;
            } else if (Input.GetKey(KeyCode.F6)) {
                this.m_MouseLook.m_MouseSensitivity = Mathf.Clamp(this.m_MouseLook.m_MouseSensitivity + Time.deltaTime * 2, MIN_MOUSE_SENSITIVITY, MAX_MOUSE_SENSITIVITY);
                this.m_PrefSaveCounter = 0.5f;
            }

            if (this.m_PrefSaveCounter > 0)
            {
                this.m_PrefSaveCounter -= Time.unscaledDeltaTime;
                if (this.m_PrefSaveCounter <= 0)
                {
                    Resources.Load<Console>(ResourcePaths.SO_CONSOLE).Log($"FIELD OF VIEW SET TO {this.m_VirtualCam.m_Lens.FieldOfView}");
                    Resources.Load<Console>(ResourcePaths.SO_CONSOLE).Log($"MOUSE LOOK SENSITIVITY SET TO {this.m_MouseLook.m_MouseSensitivity}");
                    PlayerPrefs.SetFloat(PREF_FOV, this.m_VirtualCam.m_Lens.FieldOfView);
                    PlayerPrefs.SetFloat(PREF_MOUSE_SENSITIVITY, this.m_MouseLook.m_MouseSensitivity);
                    PlayerPrefs.Save();
                }
            }
        }
    }
}