using System;
using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core;
using TheArchitect.Core.Constants;

namespace TheArchitect.SceneObjects
{
    public class SimpleMessage : SceneObject
    {
        public const string OUTCOME_CONTINUE = "CONTINUE";
        [SerializeField] private Text m_Text;
        [SerializeField] private string m_DismissButton = "Jump";

        public void MessageText(string text)
        {
            this.m_Text.text = text;
        }

        public void MessageButton(string button)
        {
            this.m_DismissButton = button;
        }


        void Update()
        {
            if (Time.deltaTime==0)
                return;

            if (Input.GetMouseButtonDown(0) || Input.GetButton(m_DismissButton))
            {
                Outcome = OUTCOME_CONTINUE;
                Destroy(this.gameObject, 0.25f);
            }
        }
    }
}