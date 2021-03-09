using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

namespace TheArchitect.SceneObjects
{
    public class TimelineUI : SceneObject
    {
        public const string OUTCOME_CLOSE = "CLOSE";
        [SerializeField] private Sprite m_SpritePlay;
        [SerializeField] private Sprite m_SpritePause;
        [SerializeField] private Button m_ButtonPlay;
        [SerializeField] private Button m_ButtonSpeed;
        [SerializeField] private Button m_ButtonClose;
        [SerializeField] private Image m_ImageProgress;
        [SerializeField] private PlayableDirector m_Director;
        [SerializeField] private float m_HideTime = 4;
        
        private Canvas m_Canvas;
        private Image m_ButtonPlayImage;
        private Text m_ButtonSpeedText;
        private Animator m_Animator;
        private float m_MouseTimer;
        private bool m_ForceHide = false;

        void Start()
        {
            this.m_Canvas = this.m_ImageProgress.GetComponentInParent<Canvas>();
            this.m_ButtonPlayImage = this.m_ButtonPlay.GetComponent<Image>();
            this.m_ButtonSpeedText = this.m_ButtonSpeed.GetComponentInChildren<Text>();
            this.m_Animator = this.GetComponent<Animator>();

            this.m_ButtonPlay.onClick.AddListener(() => {
                if ( this.m_Director.playableGraph.GetRootPlayable(0).GetSpeed() >0 )
                    this.m_Director.playableGraph.GetRootPlayable(0).SetSpeed(0);
                else
                    this.m_Director.playableGraph.GetRootPlayable(0).SetSpeed(1);
            });

            this.m_ButtonSpeed.onClick.AddListener( () => {
                double s = this.m_Director.playableGraph.GetRootPlayable(0).GetSpeed();
                this.m_Director.playableGraph.GetRootPlayable(0).SetSpeed(Mathf.Repeat((float) s - 0.25f, 1.75f));
            });

            this.m_ButtonClose.onClick.AddListener( () => Outcome = OUTCOME_CLOSE);

            this.m_MouseTimer = 0;
        }

        public void DisableUI()
        {
            this.m_ForceHide = true;
        }
        public void EnableUI()
        {
            this.m_ForceHide = false;
            this.m_MouseTimer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.deltaTime==0)
                return;

            this.m_MouseTimer += Time.deltaTime;

            if (Input.GetAxis("Mouse X")!=0 || Input.GetAxis("Mouse Y")!=0 || Input.GetMouseButton(0))
                this.m_MouseTimer = 0;
            
            this.m_Animator.SetBool("on", this.m_MouseTimer < this.m_HideTime && !m_ForceHide);

            CursorManager.RequestUnlocked();
            CursorManager.RequestVisible();

            if (Input.GetMouseButton(0))
            {
                Rect area = new Rect(m_ImageProgress.rectTransform.rect.x,
                    m_ImageProgress.rectTransform.rect.y,
                    m_ImageProgress.rectTransform.rect.width,
                    50);
                Vector2 vec;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        m_ImageProgress.rectTransform,
                        Input.mousePosition,
                        this.m_Canvas.worldCamera,
                        out vec
                );
                if (area.Contains(vec) )
                {
                    vec = Rect.PointToNormalized(m_ImageProgress.rectTransform.rect, vec);
                    this.m_Director.playableGraph.GetRootPlayable(0).SetSpeed(0);
                    this.m_Director.time = ( vec.x * m_Director.playableAsset.duration );
                }
            }

            m_ImageProgress.fillAmount = (float) ( m_Director.time / m_Director.playableAsset.duration );

            double speed = this.m_Director.playableGraph.GetRootPlayable(0).GetSpeed();
            this.m_ButtonSpeedText.text = $"{speed:0.##}x";
            this.m_ButtonSpeedText.color = speed != 1 ? Color.cyan : Color.white;
            this.m_ButtonPlayImage.sprite = this.m_Director.playableGraph.GetRootPlayable(0).GetSpeed() > 0 ? m_SpritePause : m_SpritePlay;
        }

    }

}