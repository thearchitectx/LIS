using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TheArchitect.Core.Constants;

namespace TheArchitect.Core.Data
{

    [CreateAssetMenu(fileName = "New Character", menuName = "Data/Character")]
    public class Character : ScriptableObject
    {
        public const string STAT_RELATIONSHIP = "RELATIONSHIP";
        public const string STAT_AFFINITY = "AFFINITY";
        public const string STAT_CORRUPTION = "CORRUPTION";
        public const string STAT_INTEL = "INTEL";
        public const string STAT_INTEL_ = "INTEL_{0}";

        [SerializeField] private string m_DisplayName;
        [SerializeField] private int m_Age = 18;
        [SerializeField] private Color m_Color;
        [SerializeField] private Color m_ColorContrast;
        [SerializeField] private DialogBlip m_DialogBlip;
        [SerializeField] private SkeletonType m_SkeletonType;
        [SerializeField] private string[] m_Intel = new string[0];

        public string DisplayName { get { return m_DisplayName; } }
        public int Age { get { return m_Age; } }
        public Color Color { get { return m_Color; } }
        public Color ColorContrast { get { return m_ColorContrast; } }
        public DialogBlip DialogBlip { get { return m_DialogBlip; } }
        public string[] Intel { get { return m_Intel; } }

        public Sprite Sprite
        {
            get {
                return Resources.Load<Sprite>($"{ResourcePaths.SO_CHARACTERS}/{this.name}");
            }
        }

    }

    public enum DialogBlip
    {
        Male, Female, Player
    }

}
