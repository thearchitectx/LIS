using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public enum ChoiceIcon
    {
        NONE,
        DICK_INTELLIGENCE,
        DICK_CHARISMA,
        DICK_DARING,
        DICK_KARMA_GOOD,
        DICK_KARMA_EVIL,
        SMOOTH_TALK,
        STAT_AFFINITY,
        STAT_CORRUPTION,
        AXIS_INTERACTION,
        AXIS_INTERACTION_HEART,
        ITM_JOINT,
        ITM_MONEY
    }

    public class Choice
    {
        [XmlElement("check-flag", typeof(CheckFlag)),
            XmlElement("check-perk", typeof(CheckPerk)),
            XmlElement("check-stat", typeof(CheckStat)),
            XmlElement("check-item", typeof(CheckItem)),
            XmlElement("check-text", typeof(CheckText)),
            XmlElement("check-group", typeof(CheckGroupPredicate))]
        public Predicate[] Predicates;
        [XmlAttribute("out")]
        public string Output;
        [XmlElement("text")]
        public string Text;
        [XmlElement("lock-reason")]
        public string LockReason = null;
        [XmlAttribute("icon")]
        public ChoiceIcon Icon = ChoiceIcon.NONE;
        [XmlAttribute("icon-text")]
        public string IconText = null;
        [XmlElement("then")]
        public CutsceneNode ThenNode;

        public string IconPath
        {
            get {
                if (Icon.ToString("g").StartsWith("ITM_"))
                    return $"ScriptableObjects/Items/{Icon.ToString("g")}";

                switch (Icon)
                {
                    case ChoiceIcon.DICK_INTELLIGENCE: return "UI/Sprites/dick-intelligence";
                    case ChoiceIcon.DICK_CHARISMA: return "UI/Sprites/dick-charisma";
                    case ChoiceIcon.DICK_DARING: return "UI/Sprites/dick-daring";
                    case ChoiceIcon.DICK_KARMA_GOOD: return "UI/Sprites/dick-karma-g";
                    case ChoiceIcon.DICK_KARMA_EVIL: return "UI/Sprites/dick-karma-e";
                    case ChoiceIcon.SMOOTH_TALK: return "ScriptableObjects/Perks/PRK_SMOOTH_TALKER";
                    case ChoiceIcon.AXIS_INTERACTION: return "UI/Sprites/mouse-control";
                    case ChoiceIcon.AXIS_INTERACTION_HEART: return "UI/Sprites/mouse-control-heart";
                    case ChoiceIcon.STAT_AFFINITY: return ResourcePaths.ICON_AFFINITY;
                    case ChoiceIcon.STAT_CORRUPTION: return ResourcePaths.ICON_CORRUPTION;
                    default: return null;
                }
            }
        }
    }

    public class ChoiceAction : CutsceneAction
    {
        [XmlElement("c", typeof(Choice))]
        public Choice[] Choices = null;
        [XmlAttribute("help")]
        public bool ShowHelp;
        [XmlAttribute("shuffled")]
        public bool Shuffled = false;

        [XmlIgnore] private GameObject m_Canvas;
        [XmlIgnore] private PanelChoice m_Panel;
        [XmlIgnore] private string m_SelectedChoice;
        [XmlIgnore] private CutsceneNode m_SelectedSubnode;
        

        public override void ResetState()
        {
            m_Panel = null;
            m_Canvas = null;
            m_SelectedChoice = null;
            m_SelectedSubnode = null;
        }

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            // Running m_SelectedSubnode
            if (m_SelectedSubnode != null)
            {
                var output = m_SelectedSubnode.CurrentAction.Update(cutscene, controller);
                if (output==OUTPUT_NEXT && m_SelectedSubnode.HasNextAction())
                {
                    m_SelectedSubnode.NextAction();
                    return null;
                }
                else
                {
                    return output;
                }
            }
            else if (m_Canvas == null)
            {
                this.m_Canvas = GameObject.Instantiate(Resources.Load<GameObject>(ResourcePaths.PREFAB_CANVAS_RAYCASTER));
                this.m_Canvas.transform.SetParent(controller.transform);
                
                this.m_Panel = GameObject.Instantiate(
                        Resources.Load<GameObject>(
                            ResourcePaths.PREFAB_PANEL_CHOICE
                        )
                    ).GetComponent<PanelChoice>();

                this.m_Panel.transform.SetParent(this.m_Canvas.transform, false);
                this.m_Panel.ShowHelpText = this.ShowHelp;

                if (Shuffled)
                {
                    ShuffleChoices();
                }
                for (int i=0; i < Choices.Length; i++)
                {
                    Choice c = Choices[i];
                    bool condition = Predicate.Resolve(c.Predicates);
                    if (c.LockReason !=null || condition)
                    {
                        this.m_Panel.AddChoice(
                            c.Output,
                            condition ? ResourceString.Parse(c.Text) : ResourceString.Parse(c.LockReason),
                            c.IconPath,
                            c.IconText,
                            condition);
                    }
                }
                
                this.m_Panel.onChoice.AddListener( choice => this.m_SelectedChoice = choice);
            }
            else if (this.m_SelectedChoice != null)
            {
                GameObject.Destroy(m_Canvas);
                Choice c = FindChoice(this.m_SelectedChoice);

                if (c.ThenNode != null)
                {
                    this.m_SelectedSubnode = c.ThenNode;
                    this.m_SelectedSubnode.ResetState();
                    return null;
                }
                else {
                    return this.m_SelectedChoice.StartsWith("#") ? OUTPUT_NEXT : this.m_SelectedChoice;
                }
            }
            
            return null;
        }

        public Choice FindChoice(string output)
        {
            foreach (var c in this.Choices)
            {
                if (c.Output == output)
                {
                    return c;
                }
            }

            return new Choice() { Output = OUTPUT_NEXT };
        }

        public void ShuffleChoices()
        {
             for (int i = 0; i < Choices.Length - 1; i++) 
            {
                int rnd = Random.Range(i, Choices.Length);
                var tempGO = Choices[rnd];
                Choices[rnd] = Choices[i];
                Choices[i] = tempGO;
            }
        }
        
    }
}