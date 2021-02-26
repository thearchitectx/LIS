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

        public void AddChoice(string id, string text, bool interactable)
        {
            this.m_Choices.Add(new Choice() { Id = id, Text = text, Interactable = interactable });
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
                buttonObject.GetComponentInChildren<Text>().text = c.Text;
                Button b = buttonObject.GetComponentInChildren<Button>();
                b.interactable = c.Interactable;
                b.onClick.AddListener(
                    () => {  if (Time.timeScale>0) onChoice.Invoke(c.Id); }
                );
                if (first) {
                    b.Select();
                    first = false;
                }
            }


            Destroy(this.m_ButtonTemplate);
        }

        void Update()
        {
            this.m_LastSelected = this.m_EventSystem.currentSelectedGameObject != null
                ? this.m_EventSystem.currentSelectedGameObject
                : this.m_LastSelected;

            this.m_ClickTimer -= Time.deltaTime;
            float wheel = Input.GetAxisRaw("Mouse ScrollWheel");

            if (this.m_LastSelected != null && Input.GetMouseButtonDown(0) && this.m_ClickTimer < 0)
            {
                m_LastSelected.GetComponent<Button>().onClick.Invoke();
                return;
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
