using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TheArchitect.Core.Constants;
using TheArchitect.Controllers.FirstPerson;

namespace TheArchitect.Core.Controllers
{

    public class PanelGameSettings : MonoBehaviour
    {   
        public const int ANIM_INT_IDLE = 8;
        public const int ANIM_INT_SHAPE = 4;
        public const string ANIM_TRIGGER_PHYSICS = "REACT_PISSED_OFF01";

        [SerializeField] private Transform m_Character;
        [SerializeField] private Transform m_CinemachineBoobs;
        [SerializeField] private Transform m_CharacterStand;
        [SerializeField] private Slider m_SliderSensitivity;
        [SerializeField] private Slider m_SliderShape;
        [SerializeField] private Slider m_SliderBreastStiffness;
        [SerializeField] private Slider m_SliderBreastDamping;
        [SerializeField] private Slider m_SliderBreastMass;

        public UnityEvent OnConfirm = new UnityEvent();

        private Animator m_CharacterAnimator;
        private float m_PoseTimer;
        private float m_PoseBoobs;

        void Start()
        {
            this.m_CharacterAnimator = this.m_Character.GetComponent<Animator>();
            this.m_SliderSensitivity.minValue = FirstPersonPrefHandler.MIN_MOUSE_SENSITIVITY;
            this.m_SliderSensitivity.maxValue = FirstPersonPrefHandler.MAX_MOUSE_SENSITIVITY;
            
            this.m_SliderSensitivity.SetValueWithoutNotify(PlayerPrefs.GetFloat(FirstPersonPrefHandler.PREF_MOUSE_SENSITIVITY, FirstPersonPrefHandler.DEFAULT_MOUSE_SENSITIVITY));
            this.m_SliderShape.SetValueWithoutNotify(PlayerPrefs.GetInt(CharacterInitializer.PREF_BLEND_SHAPE_ENHANCED, CharacterInitializer.DEFAULT_BLEND_SHAPE_ENHANCED));
            this.m_SliderBreastStiffness.SetValueWithoutNotify(PlayerPrefs.GetFloat(CharacterInitializer.PREF_BREAST_STIFFNESS, CharacterInitializer.DEFAULT_BREAST_STIFFNESS));
            this.m_SliderBreastDamping.SetValueWithoutNotify(PlayerPrefs.GetFloat(CharacterInitializer.PREF_BREAST_DAMPING, CharacterInitializer.DEFAULT_BREAST_DAMPING));
            this.m_SliderBreastMass.SetValueWithoutNotify(PlayerPrefs.GetFloat(CharacterInitializer.PREF_BREAST_MASS, CharacterInitializer.DEFAULT_BREAST_MASS));
        }

        public void ReadDefaults()
        {
            this.m_SliderSensitivity.value = FirstPersonPrefHandler.DEFAULT_MOUSE_SENSITIVITY;
            this.m_SliderShape.value = CharacterInitializer.DEFAULT_BLEND_SHAPE_ENHANCED;
            this.m_SliderBreastStiffness.value = CharacterInitializer.DEFAULT_BREAST_STIFFNESS;
            this.m_SliderBreastDamping.value = CharacterInitializer.DEFAULT_BREAST_DAMPING;
            this.m_SliderBreastMass.value = CharacterInitializer.DEFAULT_BREAST_MASS;
        }

        public void Confirm()
        {
            PlayerPrefs.SetFloat(FirstPersonPrefHandler.PREF_MOUSE_SENSITIVITY, this.m_SliderSensitivity.value);
            PlayerPrefs.SetInt(CharacterInitializer.PREF_BLEND_SHAPE_ENHANCED, (int)this.m_SliderShape.value);
            PlayerPrefs.SetFloat(CharacterInitializer.PREF_BREAST_STIFFNESS, this.m_SliderBreastStiffness.value);
            PlayerPrefs.SetFloat(CharacterInitializer.PREF_BREAST_DAMPING, this.m_SliderBreastDamping.value);
            PlayerPrefs.SetFloat(CharacterInitializer.PREF_BREAST_MASS, this.m_SliderBreastMass.value);
            PlayerPrefs.Save();

            OnConfirm.Invoke();
        }

        public void PhysicsTest()
        {
            this.m_CharacterAnimator.SetTrigger(ANIM_TRIGGER_PHYSICS);
        }

        public void OnCharacterShapeChange(float value)
        {
            CharacterInitializer.ApplyBlendShape(this.m_Character, SkeletonPaths.BLENDSHAPE_ENHANCED, (int) value);

            this.m_PoseTimer = 5;
        }

        public void OnBreastStiffnessChange(float value)
        {
            CharacterInitializer.SetBreastPhysicsParams(this.m_Character, value, null, null);
            this.m_PoseBoobs = 5;
        }

        public void OnBreastDampingChange(float value)
        {
            CharacterInitializer.SetBreastPhysicsParams(this.m_Character, null, value, null);
            this.m_PoseBoobs = 5;
        }

        public void OnBreastMassChange(float value)
        {
            CharacterInitializer.SetBreastPhysicsParams(this.m_Character, null, null, value);
            this.m_PoseBoobs = 5;
        }

        void Update()
        {
            CursorManager.RequestUnlocked();
            CursorManager.RequestVisible();

            this.m_PoseTimer -= Time.deltaTime;
            this.m_PoseBoobs -= Time.deltaTime;
            this.m_CharacterAnimator.SetInteger("idle", this.m_PoseTimer > 0 ? ANIM_INT_SHAPE : ANIM_INT_IDLE);
            this.m_CinemachineBoobs.gameObject.SetActive(this.m_PoseBoobs > 0);

            if (Input.GetMouseButton(1))
            {
                m_CharacterStand.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 2, 0));
            }
        }
    }

}