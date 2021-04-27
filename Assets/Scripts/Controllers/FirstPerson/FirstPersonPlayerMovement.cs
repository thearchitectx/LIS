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
        [SerializeField] private bool m_WalkEnabled = true;
        [SerializeField] private AudioClip[] m_FootStepClips;

        private CharacterController m_CharacterController;
        private AudioSource[] m_AudioSources;
        private float m_StepDist;
        private bool m_StepLeft;
        private bool m_Crouched;
        public float Gravity = -9.81f;

        void Start()
        {
            this.m_CharacterController = GetComponent<CharacterController>();
            this.m_AudioSources = GetComponents<AudioSource>();
        }

        void Update()
        {

            if (Time.deltaTime==0)
                return;
            this.m_Grounded = Physics.CheckSphere(this.transform.position, 0.1f) && this.m_Velocity.y <= 0;

            var x = m_WalkEnabled ? Input.GetAxis("Horizontal") : 0f;
            var z = m_WalkEnabled ? Input.GetAxis("Vertical") : 0f;
            var v = new Vector2(x, z).normalized;
            x = v.x;
            z = v.y;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                x *= 2;
                z *= 2;
            }


            Vector3 move = transform.right * x + transform.forward * z;

            CollisionFlags collisionMove = this.m_CharacterController.Move(move * this.m_Speed * (this.m_Crouched?0.25f:1f) * Time.deltaTime);

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
                //m_Velocity.y = -3;
            }

            // Footstep sound
            if ( this.m_Grounded && (collisionMove & CollisionFlags.Sides)==0) 
            {
                m_StepDist += (Mathf.Abs(move.x)+Mathf.Abs(move.z)) * Time.deltaTime;
                if (m_StepDist>0.6f)
                {
                    m_StepDist = 0;
                    this.m_AudioSources[m_StepLeft?0:1].PlayOneShot(this.m_FootStepClips[UnityEngine.Random.Range(0, this.m_FootStepClips.Length)]);
                    m_StepLeft = !m_StepLeft;
                }
                
            }

            this.m_CharacterController.Move(m_Velocity * Time.deltaTime);
        }
    }
}
