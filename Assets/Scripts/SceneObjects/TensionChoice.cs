using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheArchitect.SceneObjects
{
    public class TensionChoice : SceneObject
    {
        [SerializeField] private RectTransform m_ImageWheel;
        [SerializeField] private Image[] m_ImageChoices;
        [SerializeField] private Image m_ImageTimer;
        [SerializeField] private Color m_ImageTimerColorA;
        [SerializeField] private Color m_ImageTimerColorB;

        private Text[] m_TextChoices;
        private string[] m_Texts = new string[4];

        private float m_SpinPos;
        private float m_SpinTimer;
        private float m_SpinAcceleration;
        private float m_SpinSpeed;
        [SerializeField] private float m_SpinMinAcceleration = 3;
        [SerializeField] private float m_SpinMaxAcceleration = 5;
        [SerializeField] private float m_SpinMinTimer = 1;
        [SerializeField] private float m_SpinMaxTimer = 2;

        private float m_TimerLimit = 0;
        [SerializeField] private float m_TimeLimit = 10;

        private float m_SelectedAngle = 0;

        public void MessageChoice0(string text)
        {
            this.m_Texts[0] = text;
        }
        public void MessageChoice1(string text)
        {
            this.m_Texts[1] = text;
        }
        public void MessageChoice2(string text)
        {
            this.m_Texts[2] = text;
        }
        public void MessageChoice3(string text)
        {
            this.m_Texts[3] = text;
        }
        public void MessageTimeLimit(string timeAsString)
        {
            float.TryParse(timeAsString, out this.m_TimeLimit);
        }

        // Start is called before the first frame update
        void Start()
        {
            this.Outcome = null;
            this.m_TextChoices = new Text[m_ImageChoices.Length];

            for ( var i=0; i < this.m_ImageChoices.Length; i++)
                this.m_TextChoices[i] = this.m_ImageChoices[i].GetComponentInChildren<Text>();

            for (var i=0; i<this.m_Texts.Length; i++)
                this.m_TextChoices[i].text = this.m_Texts[i];

            this.m_SpinTimer = 0;
            this.m_SpinAcceleration = 100;
            this.m_SpinPos = Random.Range(0, 360);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.deltaTime==0)
                return;
            
            if (this.Outcome != null)
            {
                m_SpinPos = Mathf.MoveTowards(m_SpinPos, m_SelectedAngle, Time.deltaTime * 100);
                this.m_ImageWheel.rotation = Quaternion.Euler(0, 0, m_SpinPos);
                return;
            }

            this.m_TimerLimit += Time.deltaTime;
            this.m_ImageTimer.fillAmount = Mathf.InverseLerp(0, this.m_TimeLimit, this.m_TimerLimit);
            this.m_ImageTimer.color = Color.Lerp(m_ImageTimerColorA, m_ImageTimerColorB, this.m_ImageTimer.fillAmount);

            this.m_SpinTimer -= Time.deltaTime;
            if (this.m_SpinTimer<=0)
            {
                float s = this.m_SpinSpeed>0?-1:1;
                this.m_SpinAcceleration = s * UnityEngine.Random.Range(m_SpinMinAcceleration, m_SpinMaxAcceleration);
                this.m_SpinTimer = UnityEngine.Random.Range(m_SpinMinTimer, m_SpinMaxTimer);
            }

            float p = Mathf.Repeat(this.m_SpinPos, 360);
            int selected;
            if (p > 0 && p <= 90) {
                selected = 2;
                m_SelectedAngle = 45;
            } else if (p > 90 && p <= 180) {
                selected = 3;
                m_SelectedAngle = 135;
            } else if (p > 180 && p <= 270) {
                selected = 1;
                m_SelectedAngle = 225;
            } else {
                m_SelectedAngle = 315;
                selected = 0;
            }
                
            for (var i=0; i<this.m_ImageChoices.Length; i++)
            {
                if (i!=selected)
                {
                    this.m_TextChoices[i].color = Color.white;
                    this.m_TextChoices[i].color = new Color32(0xff, 0xff, 0xff, 0x88);
                    this.m_ImageChoices[i].color = new Color32(0xff, 0xff, 0xff, 0x88);
                    this.m_ImageChoices[i].rectTransform.localScale = new Vector3(
                        Mathf.Clamp(this.m_ImageChoices[i].rectTransform.localScale.x-Time.deltaTime, 1, 1.1f),
                        Mathf.Clamp(this.m_ImageChoices[i].rectTransform.localScale.y-Time.deltaTime, 1, 1.1f),
                        Mathf.Clamp(this.m_ImageChoices[i].rectTransform.localScale.z-Time.deltaTime, 1, 1.1f)
                    );
                }
            }

            this.m_TextChoices[selected].color = Color.cyan;
            this.m_ImageChoices[selected].color = Color.cyan;
            this.m_ImageChoices[selected].rectTransform.localScale = Vector2.Lerp(
                Vector2.one,
                new Vector2(1.1f, 1.1f),
                Mathf.Clamp(this.m_SpinTimer, 0, 0.25f)
            );


            this.m_SpinSpeed += this.m_SpinAcceleration;
            this.m_SpinPos += Time.deltaTime * this.m_SpinSpeed;
            this.m_ImageWheel.rotation = Quaternion.Euler(new Vector3(0, 0, this.m_SpinPos));

            if (this.m_TimerLimit >= 0.75f && (Input.GetMouseButtonDown(0) || Input.GetButton("Jump") || this.m_TimerLimit >= this.m_TimeLimit) )
            {
                for (var i=0; i<this.m_ImageChoices.Length; i++)
                    if (i!=selected)
                        this.m_ImageChoices[i].gameObject.SetActive(false);

                this.m_SpinPos = Mathf.Repeat(this.m_SpinPos, 360);
                this.m_ImageTimer.transform.parent.gameObject.SetActive(false);

                Outcome = selected.ToString();
            }
        }

    }
}