using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TheArchitect.Core.Data.Variables;

namespace TheArchitect.Core.Controllers
{
    struct Choice
    {
        public string Id;
        public string Text;
        public string Icon;
        public string IconText;
        public bool Interactable;
    }

    public class ChoiceEvent : UnityEvent<string> { }

    public class PanelChoice : MonoBehaviour
    {
        [SerializeField] private Transform m_Help;
        public ChoiceEvent onChoice = new ChoiceEvent();
        private GameObject m_ButtonTemplate;
        private GameObject m_LastSelected;
        private List<Choice> m_Choices = new List<Choice>();
        private EventSystem m_EventSystem;
        public bool ShowHelpText = false;
        public float m_WheelTimer;
        public float m_ClickTimer = 1f;
        public FloatVariable m_CharacterTimeInterval;

        public void AddChoice(string id, string text, string icon, string iconText, bool interactable)
        {
            this.m_Choices.Add(new Choice() { Id = id, Text = text, Icon = icon, IconText = iconText, Interactable = interactable });
        }

        private System.Collections.IEnumerator CoroutineDisplayText(Text text, string message)
        {
            string s = "";
            for (int i=0; i<message.Length; i++)
            {
                if (message[i] == '<')
                {
                    text.text = message;
                    break;
                }
                else
                {
                    s += message[i];
                    text.text = s;
                    yield return new WaitForSeconds(m_CharacterTimeInterval.Value);
                }
            }

        }

        void Start()
        {
            this.m_ButtonTemplate = this.transform.GetComponentInChildren<Button>().gameObject;
            this.m_EventSystem = GameObject.FindObjectOfType<EventSystem>();

            if (!this.ShowHelpText)
            {
                Destroy(this.m_Help.gameObject);
            }

            bool first = true;
            foreach (var c in this.m_Choices)
            {
                GameObject buttonObject = GameObject.Instantiate(this.m_ButtonTemplate);
                buttonObject.transform.SetParent(this.transform, false);
                StartCoroutine(CoroutineDisplayText(buttonObject.GetComponentInChildren<Text>(), c.Text));

                Button b = buttonObject.GetComponentInChildren<Button>();
                b.interactable = c.Interactable;
                b.onClick.AddListener(
                    () => {  if (Time.timeScale>0) onChoice.Invoke(c.Id); }
                );
                if (first) {
                    b.Select();
                    first = false;
                }
                
                Image imageIcon = buttonObject.transform.Find("Image Icon").GetComponent<Image>();
                if (c.Icon != null && c.Icon != "")
                    imageIcon.sprite = Resources.Load<Sprite>(c.Icon);
                imageIcon.gameObject.SetActive(imageIcon.sprite);
                Text textIcon = buttonObject.transform.Find("Text Icon").GetComponent<Text>();
                if (!string.IsNullOrEmpty(c.IconText) && !string.IsNullOrEmpty(c.Icon))
                    textIcon.text = c.IconText;
                Image imageShadow = imageIcon.transform.Find("Image Shadow").GetComponent<Image>();

                imageIcon.gameObject.SetActive(imageIcon.sprite);
                textIcon.gameObject.SetActive(!string.IsNullOrEmpty(c.IconText));
                imageShadow.gameObject.SetActive(!string.IsNullOrEmpty(c.IconText));

                // Fix padding for icon
                if (imageIcon.sprite!=null) 
                {
                    LayoutGroup layout = b.GetComponentInChildren<LayoutGroup>();
                    RectOffset padding = new RectOffset(layout.padding.left+32, layout.padding.right, layout.padding.top, layout.padding.bottom);
                    layout.padding = padding;
                }

            }


            Destroy(this.m_ButtonTemplate);
        }

        void Update()
        {
            if (Time.deltaTime==0)
                return;

            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.U) && Input.GetKey(KeyCode.LeftControl))
                foreach (var b in this.GetComponentsInChildren<Button>())
                    b.interactable = true;
            #endif
                
            this.m_LastSelected = this.m_EventSystem.currentSelectedGameObject != null
                ? this.m_EventSystem.currentSelectedGameObject
                : this.m_LastSelected;

            this.m_ClickTimer -= Time.deltaTime;
            float wheel = Input.GetAxisRaw("Mouse ScrollWheel");

            if (this.m_LastSelected != null && Input.GetMouseButtonDown(0) && this.m_ClickTimer < 0)
            {
                Button b = m_LastSelected.GetComponent<Button>();
                if (b.interactable)
                {
                    b.onClick.Invoke();
                    return;
                }
            }

            if (this.m_EventSystem.currentSelectedGameObject == null && !this.m_EventSystem.alreadySelecting && (Input.GetMouseButtonDown(0) ||  Input.GetAxis("Vertical") != 0 || wheel !=0) )
            {
                this.GetComponentInChildren<Button>().Select();
                return;
            }

            if (this.m_EventSystem.currentSelectedGameObject && wheel!=0 && this.m_WheelTimer>0.15f)
            {
                Selectable b = this.m_EventSystem.currentSelectedGameObject.GetComponent<Selectable>();
                Selectable next = wheel > 0 ? b.FindSelectableOnUp() : b.FindSelectableOnDown();
                if (next!=null)
                {
                    next.Select();
                    this.m_WheelTimer = 0;
                }
            }

            this.m_WheelTimer += Time.deltaTime;
        }

    }
}
