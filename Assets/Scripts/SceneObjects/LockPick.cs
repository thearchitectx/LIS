using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheArchitect.SceneObjects
{
    struct TumblerControl
    {
        public Transform Root;
        public Transform HandleBone;
        public Transform TumblerBone;
        public SkinnedMeshRenderer HandleRenderer;
        public float TargetY;
        public Vector3 Velocity;
        public float MaxSpeed;
    }

    public class LockPick : SceneObject
    {
        const float TUMBLER_DIMENSION = 0.15f;
        const float TUMBLER_REST_Y  = 0.0008628154f;
        public const string OUTCOME_UNLOCKED = "UNLOCKED";
        public const string OUTCOME_QUIT = "QUIT";
        [SerializeField] private int m_TumblerCount;
        [SerializeField] private Transform m_BoxFArmObject;
        [SerializeField] private Transform m_BoxB;
        [SerializeField] private Transform m_Pick;
        [SerializeField] private Transform m_PickTouchPosition;
        [SerializeField] private GameObject m_TumblerPrefab;
        [SerializeField] private float m_PickSpeed = 0.01f;
        
        [SerializeField] private int[] m_TumblerSequence;
        [SerializeField] private TumblerControl[] m_Tumblers;
        
        private int m_SequencePosition = -1;
        private bool m_InputEnabled = true;

        public void MsgGenerate(string sizeAsString)
        {
            int size;
            if (int.TryParse(sizeAsString, out size))
                Generate(size);
        }

        public void DisableInput() { m_InputEnabled = false;}
        public void EnableInput() { m_InputEnabled = true; }

        public void Generate(int size)
        {
            this.m_TumblerCount = size;
            this.m_TumblerSequence = new int[size];
            this.m_TumblerSequence.Fill(-1);
            this.m_TumblerSequence[0] = size/2;

            Debug.Log(this.m_TumblerSequence[0]);
            for (int i=1; i<size; i++)
            {
                int p = this.m_TumblerSequence[i-1];
                int dir = UnityEngine.Random.value > 0.5f ? -1 : 1;
                int x = dir;
                while (Array.IndexOf( this.m_TumblerSequence, p+x) >= 0 && p+x>=0 && p+x < size  ) 
                    x += dir;

                if (p+x<0 || p+x>=size)
                {
                    dir = -dir;
                    x = dir;
                    while (Array.IndexOf( this.m_TumblerSequence, p+x) >= 0 && p+x>=0 && p+x < size  ) 
                        x += dir;
                }

                this.m_TumblerSequence[i] = p+x;
            }
            
            this.m_BoxB.transform.position += new Vector3(-TUMBLER_DIMENSION * (m_TumblerCount-1), 0, 0);
            this.m_Tumblers = new TumblerControl[m_TumblerCount];

            for (int i=0; i<this.m_TumblerCount; i++)
            {
                GameObject tumbler = Instantiate(this.m_TumblerPrefab);
                tumbler.transform.SetParent(this.transform, false);
                tumbler.transform.localPosition += new Vector3(-TUMBLER_DIMENSION * i, 0, 0);

                this.m_Tumblers[i] = new TumblerControl() {
                    Root = tumbler.transform,
                    HandleBone = tumbler.transform.Find("ArmObject/Root/Turn/Handle"),
                    TumblerBone = tumbler.transform.Find("ArmObject/Root/Tumbler"),
                    HandleRenderer = tumbler.transform.Find("SK_LockTumbler_Turn").GetComponent<SkinnedMeshRenderer>(),
                    TargetY = TUMBLER_REST_Y,
                    Velocity = Vector3.zero
                };
            }
        }

        void Start() 
        {
            if (this.m_Tumblers==null)
                Generate(5);
            OneTimeAudioPlayer.GetAndCache("unlock");
            OneTimeAudioPlayer.GetAndCache("metal-click");
            OneTimeAudioPlayer.GetAndCache("knock-metal");
        }

        void Update()
        {
            if (Time.deltaTime==0)
                return;

            if (this.m_Tumblers==null)
                return;

            if (m_InputEnabled)
            {
                float x = -Input.GetAxis("Mouse X") + (Input.GetAxis("Horizontal") * Time.deltaTime * 10);
                this.m_Pick.localPosition += new Vector3(x * m_PickSpeed, 0, 0);

                if (Input.GetMouseButtonDown(1))
                {
                    Outcome = OUTCOME_QUIT;
                    m_InputEnabled = false;
                }
            }

            int found = -1;
            for (int i=0; i<this.m_TumblerCount; i++)
            {
                if ( (this.m_Tumblers[i].HandleBone.position - this.m_PickTouchPosition.position).magnitude <= TUMBLER_DIMENSION / 7 )
                {
                    this.m_Tumblers[i].HandleRenderer.materials[1].color = Color.green;
                    this.m_Pick.localPosition = new Vector3(this.m_Pick.localPosition.x, Mathf.Clamp(this.m_Pick.localPosition.y - Time.deltaTime * 0.2f, -0.01f, 0f), this.m_Pick.localPosition.z);
                    found = i;
                } else {
                    this.m_Tumblers[i].HandleRenderer.materials[1].color = new Color32(0xCC, 0xB1, 0x86, 0xff);
                }
            }

            if (found<0)
                this.m_Pick.localPosition = new Vector3(this.m_Pick.localPosition.x, Mathf.Clamp(this.m_Pick.localPosition.y + Time.deltaTime * 0.2f, -0.01f, 0), this.m_Pick.localPosition.z);
            else if (Input.GetMouseButtonDown(0) && m_InputEnabled && this.m_SequencePosition < this.m_TumblerCount - 1)
            {
                this.GetComponent<Animator>().SetTrigger("pick");
                OneTimeAudioPlayer.Play("metal-click");
                m_SequencePosition++;
                if (this.m_TumblerSequence[this.m_SequencePosition] == found)
                {
                    this.m_Tumblers[found].Root.GetComponent<Animator>().SetTrigger("pick");
                    this.m_Tumblers[found].TargetY = 0.0025f + UnityEngine.Random.Range(-0.001f, 0.001f);
                    this.m_Tumblers[found].MaxSpeed = 0.5f;
                } else {
                    OneTimeAudioPlayer.Play("knock-metal");
                    this.m_Tumblers[found].Root.GetComponent<Animator>().SetTrigger("pick-wrong");
                    for (int i=0; i<this.m_TumblerCount; i++) {
                        this.m_Tumblers[i].TargetY = TUMBLER_REST_Y;
                        this.m_Tumblers[i].MaxSpeed = 50;
                    }
                    m_SequencePosition = -1;
                }

                if (m_SequencePosition>=m_TumblerCount-1)
                {
                    Outcome = OUTCOME_UNLOCKED;
                    m_InputEnabled = false;
                    this.GetComponent<Animator>().SetTrigger("success");
                    for (int i=0; i<this.m_TumblerCount; i++) {
                        this.m_Tumblers[i].Root.GetComponent<Animator>().SetTrigger("success");
                    }
                    OneTimeAudioPlayer.Play("unlock");
                }
            }

            for (int i=0; i<this.m_TumblerCount; i++)
            {
                this.m_Tumblers[i].TumblerBone.localPosition = 
                    Vector3.SmoothDamp(
                        this.m_Tumblers[i].TumblerBone.localPosition,
                        new Vector3(this.m_Tumblers[i].TumblerBone.localPosition.x, this.m_Tumblers[i].TargetY, this.m_Tumblers[i].TumblerBone.localPosition.z),
                        ref this.m_Tumblers[i].Velocity,
                        0.1f,
                        this.m_Tumblers[i].MaxSpeed
                    );
            }
        }
        
    }

    public static class ArrayExtensions {
        public static void Fill<T>(this T[] originalArray, T with) {
            for(int i = 0; i < originalArray.Length; i++){
                originalArray[i] = with;
        }
    }  
}
}