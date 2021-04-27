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

        public void MessageText(string text)
        {
            this.m_Text.text = text;
        }


        void Update()
        {
            if (Time.deltaTime==0)
                return;

            if (Input.GetMouseButtonDown(0) || Input.GetButton("Jump"))
            {
                Outcome = OUTCOME_CONTINUE;
                Destroy(this.gameObject, 0.25f);
            }
        }
    }
}