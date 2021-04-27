using System;
using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core;
using TheArchitect.Core.Constants;

namespace TheArchitect.SceneObjects
{
    public class FlagGauge : SceneObject
    {
        [SerializeField] private Image m_ImageBG;
        [SerializeField] private Image m_ImageValue;
        [SerializeField] private Text m_TextLabel;
        [SerializeField] private int m_MinValue = 0;
        [SerializeField] private int m_MaxValue = 100;
        [SerializeField] private string m_Flag = "FLAG";
        [SerializeField] private Color32 m_MinColor = new Color(0xFF, 0xEC, 0x00);
        [SerializeField] private Color32 m_MaxColor = new Color(0xFF, 0x00, 0xA1);

        private Game m_Game;
        private int m_LastStageChangeCount = int.MinValue;
        private float m_CurrentValue;

        public void MessageFlag(string flag)
        {
            this.m_Flag = flag;
        }

        public void MessageLabel(string label)
        {
            this.m_TextLabel.text = label;
        }

        public void MessageMinValue(string min)
        {
            int.TryParse(min, out this.m_MinValue);
        }

        public void MessageMaxValue(string max)
        {
            int.TryParse(max, out this.m_MaxValue);
        }

        public void MessageMinColor(string color)
        {
            Color c;
            if ( ColorUtility.TryParseHtmlString(color, out c) )
                this.m_MinColor = c;
        }

        public void MessageMaxColor(string color)
        {
            Color c;
            if ( ColorUtility.TryParseHtmlString(color, out c) )
                this.m_MaxColor = c;
        }

        void Start()
        {
            this.m_Game = Resources.Load<Game>(ResourcePaths.SO_GAME);
        }

        void Update()
        {
            if (this.m_LastStageChangeCount != this.m_Game.StateChangeCount)
            {
                this.m_LastStageChangeCount = this.m_Game.StateChangeCount;
                this.m_CurrentValue = Mathf.InverseLerp(this.m_MinValue, this.m_MaxValue, this.m_Game.GetFlagState(m_Flag));
            }

            this.m_ImageValue.fillAmount = Mathf.MoveTowards(this.m_ImageValue.fillAmount, this.m_CurrentValue, Time.deltaTime * 0.25f);
            this.m_ImageValue.color = Color32.Lerp(this.m_MinColor, this.m_MaxColor, this.m_ImageValue.fillAmount);
        }
    }
}