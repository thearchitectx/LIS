using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core.Data.Variables;

namespace TheArchitect.Core.Controllers
{
    public class PanelConsole : MonoBehaviour
    {
        public static string Debug;

        [SerializeField] private Image m_ImagePopup;
        [SerializeField] private Image m_ContrastImage;
        [SerializeField] private Text m_TextMessages;
        [SerializeField] private Text m_TextDebug;
        [SerializeField] private Console m_Console;

        private int m_LastCount = -1;
        private float m_RemovalTimer = 0;

        void Start()
        {
            m_ImagePopup.color = new Color(1, 1, 1, 0);
        }

        void Update()
        {
            var count = this.m_Console.Messages.Count;

            if (this.m_Console.ImagePopup != null)
            {
                m_ImagePopup.sprite = this.m_Console.ImagePopup;
                m_ImagePopup.color = new Color(1, 1, 1, Mathf.Clamp01(m_ImagePopup.color.a + (Time.deltaTime*2)) );
            }
            else
            {
                m_ImagePopup.color = new Color(1, 1, 1, Mathf.Clamp01(m_ImagePopup.color.a - (Time.deltaTime*2)) );
            }

            m_ImagePopup.gameObject.SetActive(m_ImagePopup.color.a > 0);

            if (count != this.m_LastCount)
            {
                m_TextMessages.text = string.Join("\n", this.m_Console.Messages);

                if (this.m_RemovalTimer <= 0)
                {
                    this.m_RemovalTimer = m_Console.DiscardTime;
                }
            }

            m_ContrastImage.gameObject.SetActive(count > 0);

            if (count > 0 || this.m_Console.ImagePopup != null)
            {
                this.m_RemovalTimer -= Time.deltaTime;

                if (this.m_RemovalTimer <= 0)
                {
                    this.m_Console.PurgeOldest();
                }
            }

            this.m_LastCount = count;

            if (this.m_Console.Messages != null)
            {
                this.m_TextDebug.gameObject.SetActive(true);
                this.m_TextDebug.text = this.m_Console.DebugMessage;
            }
            else
            {
                this.m_TextDebug.gameObject.SetActive(false);
            }

            if (Debug!=null)
            {
                this.m_TextDebug.gameObject.SetActive(true);
                this.m_TextDebug.text = Debug;
            }
        }

    }
}