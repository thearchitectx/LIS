using UnityEngine;
using PREF =  TheArchitect.Controllers.FirstPerson.FirstPersonPrefHandler;

namespace TheArchitect.Controllers.FirstPerson
{
    public class FirstPersonMouseLook : MonoBehaviour
    {
        [SerializeField] [Range(PREF.MIN_MOUSE_SENSITIVITY, PREF.MAX_MOUSE_SENSITIVITY)]
        public float m_MouseSensitivity = PREF.DEFAULT_MOUSE_SENSITIVITY;
        [SerializeField]
        public Transform m_Camera;

        private float m_XRotation = 0;

        void Update()
        {
            if (Time.deltaTime==0)
                return;
                
            float x = Input.GetAxis("Mouse X") * m_MouseSensitivity;
            float y = Input.GetAxis("Mouse Y") * m_MouseSensitivity;
            
            transform.Rotate(Vector3.up, x);

            this.m_XRotation -= y;
            this.m_XRotation = Mathf.Clamp(m_XRotation, -90, 90);

            m_Camera.localRotation = Quaternion.Euler(m_XRotation, 0, 0);
            
        }
    }

}
