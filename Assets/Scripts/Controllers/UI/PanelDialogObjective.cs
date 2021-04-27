using System.Xml.Serialization;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core.Data;
using TheArchitect.Core.Data.Variables;

namespace TheArchitect.Core.Controllers
{
    public enum DialogStyle
    {
        [XmlEnum(Name="default")]
        DEFAULT,
        [XmlEnum(Name="whisper-subjective")]
        WHISPER_SUBJECTIVE,
        [XmlEnum(Name="whisper")]
        WHISPER,
        [XmlEnum(Name="remote")]
        REMOTE,
        [XmlEnum(Name="remote-subjective")]
        REMOTE_SUBJECTIVE,
        [XmlEnum(Name="subjective")]
        SUBJECTIVE
    }

    public class PanelDialogObjective : MonoBehaviour
    {
        const int PADDING_MESSAGE_LEFT = 15;
        const int PADDING_MESSAGE_RIGHT = 15;
        const int PADDING_MESSAGE_BOTTOM = 5;
        const int PADDING_MESSAGE_TOP = 0;

        [SerializeField] public Transform TrackedTransform = null;
        [SerializeField] private Sprite m_SpriteLeft;
        [SerializeField] private Sprite m_SpriteRight;
        [SerializeField] private Sprite m_SpriteSubjective;
        [SerializeField] public bool ClampToScreen = true;
        [SerializeField] public Image m_ImageForward = null;
        [SerializeField] public Image m_ImageDialogBack = null;
        [SerializeField] public Image m_ImageCharacterName = null;
        [SerializeField] public Text m_TextMessage = null;
        [SerializeField] public Text m_TextName = null;
        [SerializeField] public AudioSource m_BlipAudioSource = null;

        [SerializeField] public IntVariable m_MessageDefaultFontSize;
        [SerializeField] public IntVariable m_MessageDefaultFontStyle;
        [SerializeField] public ColorVariable m_MessageDefaultFontColor;
        [SerializeField] public IntVariable m_MessageWhisperFontSize;
        [SerializeField] public IntVariable m_MessageWhisperFontStyle;
        [SerializeField] public ColorVariable m_MessageWhisperFontColor;
        [SerializeField] public IntVariable m_MessageRemoteFontSize;
        [SerializeField] public IntVariable m_MessageRemoteFontStyle;
        [SerializeField] public ColorVariable m_MessageRemoteFontColor;
        [SerializeField] public FloatVariable m_CharacterTimeInterval;

        private string m_Message = null;
        private int m_MessagePosition = 0;
        private RectTransform m_RectTransform;
        private RectTransform m_CanvasTransform;
        private bool m_IsRollingText;
        private DialogStyle m_Style;

        public bool IsRollingText { get { return m_IsRollingText; } }

        public void Display(Character character, string message, DialogStyle style = DialogStyle.DEFAULT)
        {
            this.m_Style = style;
            switch (m_Style) {
                case DialogStyle.WHISPER:
                case DialogStyle.WHISPER_SUBJECTIVE:
                    m_TextMessage.fontSize = m_MessageWhisperFontSize.Value;
                    m_TextMessage.fontStyle = (FontStyle) m_MessageWhisperFontStyle.Value;
                    m_TextMessage.color = m_MessageWhisperFontColor.Value;
                    break;
                case DialogStyle.REMOTE:
                case DialogStyle.REMOTE_SUBJECTIVE:
                    m_TextMessage.fontSize = m_MessageRemoteFontSize.Value;
                    m_TextMessage.fontStyle = (FontStyle) m_MessageRemoteFontStyle.Value;
                    m_TextMessage.color = m_MessageRemoteFontColor.Value;
                    break;
                default:
                    m_TextMessage.fontSize = m_MessageDefaultFontSize.Value;
                    m_TextMessage.fontStyle = (FontStyle) m_MessageDefaultFontStyle.Value;
                    m_TextMessage.color = m_MessageDefaultFontColor.Value;
                    break;
            }

            this.m_ImageCharacterName.gameObject.SetActive(character!=null);
            this.m_TextName.gameObject.SetActive(character!=null);
            if (character!=null)
            {
                this.m_ImageCharacterName.color = character.Color;
                this.m_TextName.color = character.ColorContrast;
                this.m_TextName.text = character.DisplayName;
            }

            StopAllCoroutines();
            StartCoroutine(_DisplayTextMessage(character, message));
        }

        public void SkipDisplay()
        {
            this.m_MessagePosition = this.m_Message.Length - 1;
        }

        public bool HasRunningDisplay()
        {
            return this.m_MessagePosition < this.m_Message.Length;
        }

        void Start()
        {
            this.m_RectTransform = GetComponent<RectTransform>();
            var canvas = GetComponentInParent<Canvas>();
            this.m_CanvasTransform = canvas.transform.GetComponent<RectTransform>();
            this.m_ImageForward.gameObject.SetActive(false);
            this.m_ImageDialogBack.sprite = null;
            this.m_RectTransform.anchoredPosition = new Vector2(-10000, -100000);
        }

        void Update()
        {
            if (Time.deltaTime==0)
            {
                m_BlipAudioSource.enabled = false;
                return;
            }

            if (this.TrackedTransform != null && this.m_Style != DialogStyle.SUBJECTIVE)
            {
                Vector3 pos = Camera.main.WorldToViewportPoint(TrackedTransform.position);
                pos = new Vector2(
                    pos.z >= 0 ? pos.x * this.m_CanvasTransform.rect.width : 0,
                    (1 - pos.y) * this.m_CanvasTransform.rect.height
                );

                bool right = pos.x - 50 > this.m_CanvasTransform.rect.width / 2;

                pos = new Vector2(
                    right ? pos.x - this.m_RectTransform.rect.width : pos.x,
                    - pos.y
                );

                if (this.ClampToScreen)
                {
                    pos.y = Mathf.Clamp(pos.y, -this.m_CanvasTransform.rect.height+m_ImageDialogBack.rectTransform.rect.height+20 , -20 );
                    pos.x = Mathf.Clamp(pos.x, 0, this.m_CanvasTransform.rect.width - m_ImageDialogBack.rectTransform.rect.width);
                }

                this.m_RectTransform.anchoredPosition = pos;
                this.m_ImageForward.rectTransform.anchoredPosition = new Vector2(
                    right ? 325 : 350,
                    right ? 10 : 0
                );
                
                var spr = right ? m_SpriteRight : m_SpriteLeft;
                if (spr != this.m_ImageDialogBack.sprite)
                {
                    this.m_ImageDialogBack.sprite = spr;
                    m_ImageDialogBack.GetComponent<LayoutGroup>().padding = new RectOffset(
                        (int) this.m_ImageDialogBack.sprite.border.x + PADDING_MESSAGE_LEFT,
                        (int) this.m_ImageDialogBack.sprite.border.z + PADDING_MESSAGE_RIGHT,
                        (int) this.m_ImageDialogBack.sprite.border.w + PADDING_MESSAGE_TOP,
                        (int) this.m_ImageDialogBack.sprite.border.y + PADDING_MESSAGE_BOTTOM
                    );
                }
            }
            else
            {
                var spr = (this.m_Style == DialogStyle.SUBJECTIVE || this.m_Style == DialogStyle.REMOTE_SUBJECTIVE || this.m_Style == DialogStyle.WHISPER_SUBJECTIVE)  ? this.m_SpriteSubjective : this.m_SpriteLeft;
                if (spr != this.m_ImageDialogBack.sprite)
                {
                    this.m_ImageDialogBack.sprite = spr;
                    m_ImageDialogBack.GetComponent<LayoutGroup>().padding = new RectOffset(
                        (int) this.m_ImageDialogBack.sprite.border.x + PADDING_MESSAGE_LEFT,
                        (int) this.m_ImageDialogBack.sprite.border.z + PADDING_MESSAGE_RIGHT,
                        (int) this.m_ImageDialogBack.sprite.border.w + PADDING_MESSAGE_TOP,
                        (int) this.m_ImageDialogBack.sprite.border.y + PADDING_MESSAGE_BOTTOM
                    );
                }
                this.m_ImageForward.rectTransform.anchoredPosition = new Vector2(355, 0);
                this.m_RectTransform.anchoredPosition = new Vector2(
                    (this.m_CanvasTransform.rect.width / 2) - ( m_ImageDialogBack.rectTransform.rect.width / 2),
                    - this.m_CanvasTransform.rect.height + m_ImageDialogBack.rectTransform.rect.height + 25
                );

            }

            this.m_ImageDialogBack.enabled = true;
        }

        IEnumerator _DisplayTextMessage(Character character, string message)
        {
            this.m_Message = message;
            this.m_MessagePosition = 1;


            string path = character != null ?
                $"SFX/Dialog/{character.DialogBlip.ToString()}"
                : $"SFX/Dialog/Player";
            this.m_BlipAudioSource.clip = Resources.Load<AudioClip>(path);
            this.m_ImageForward.gameObject.SetActive(false);

            if (this.m_BlipAudioSource.clip == null)
            {
                Debug.LogWarning($"Can't find '{path}'");
            }

            if (this.m_CharacterTimeInterval.Value == 0)
            {
                this.m_MessagePosition = this.m_Message.Length;
            }

            try
            {
                while (this.m_MessagePosition < this.m_Message.Length)
                {
                    char c = this.m_Message[this.m_MessagePosition];
                    this.m_IsRollingText  = !(c ==',' || c=='.' || c=='!' || c=='?');
                    this.m_BlipAudioSource.enabled = m_IsRollingText;
                    this.m_TextMessage.text = this.m_Message.Substring(0, this.m_MessagePosition++);
                    yield return new WaitForSeconds(m_CharacterTimeInterval.Value * (m_IsRollingText?1:4));
                }
            } 
            finally
            {
                this.m_IsRollingText = false;
                this.m_TextMessage.text = this.m_Message;
                this.m_BlipAudioSource.enabled = false;
                this.m_MessagePosition = this.m_Message.Length;
                this.m_ImageForward.gameObject.SetActive(true);
            }
        }

    }
}

