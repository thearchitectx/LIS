using UnityEngine;
using Cinemachine;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Data.Variables;

namespace TheArchitect.Controllers.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonPlayerMovement : MonoBehaviour
    {
        [SerializeField] public Transform m_Camera;
        [SerializeField] private float m_Speed = 2;
        [SerializeField] public float m_CrouchY = 0.5f;
        [SerializeField] public float m_StandY = 1.3f;
        [SerializeField] public float m_CrouchSpeed = 2;
        [SerializeField] public float m_JumpSpeed = 10;
        [SerializeField] private LayerMask m_GroundedLayerMask;
        [SerializeField] private Vector3 m_Velocity;
        [SerializeField] private bool m_Grounded;

        private CharacterController m_CharacterController;
        private bool m_Crouched;
        public float Gravity = -9.81f;

        void Start()
        {
            this.m_CharacterController = GetComponent<CharacterController>();
        }

        void Update()
        {

            if (Time.deltaTime==0)
                return;
            this.m_Grounded = Physics.CheckSphere(this.transform.position, 0.1f, m_GroundedLayerMask) && this.m_Velocity.y <= 0;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");


            Vector3 move = transform.right * x + transform.forward * z;

            this.m_CharacterController.Move(move * this.m_Speed * (this.m_Crouched?0.25f:1f) * Time.deltaTime);

            this.m_Crouched = Input.GetButton("Circle");

            this.m_Camera.localPosition = Vector3.MoveTowards(
                this.m_Camera.localPosition,
                new Vector3(0, this.m_Crouched ? this.m_CrouchY : this.m_StandY, 0),
                Time.deltaTime * this.m_CrouchSpeed
            );

            if (!this.m_Grounded) {
                m_Velocity.y += Gravity * Time.deltaTime;
            } else if (Input.GetButtonDown("Jump")) {
                m_Velocity.y = this.m_JumpSpeed;
            } else {
                m_Velocity.y = -3;
            }

            this.m_CharacterController.Move(m_Velocity * Time.deltaTime);
        }
    }
}
