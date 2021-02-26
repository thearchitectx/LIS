using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class Choice
    {
        [XmlElement("check-flag", typeof(CheckFlag)),
         XmlElement("check-stat", typeof(CheckStat)),
         XmlElement("check-item", typeof(CheckItem)),
         XmlElement("check-perk", typeof(CheckPerk))]
        public Predicate[] Predicates;
        [XmlAttribute("out")]
        public string Output;
        [XmlElement("text")]
        public string Text;
        [XmlElement("lock-reason")]
        public string LockReason = null;
        [XmlElement("then")]
        public CutsceneNode ThenNode;
    }

    public class ChoiceAction : CutsceneAction
    {
        [XmlElement("c", typeof(Choice))]
        public Choice[] Choices = null;
        [XmlAttribute("help")]
        public bool ShowHelp;

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

                for (int i=0; i < Choices.Length; i++)
                {
                    Choice c = Choices[i];
                    bool condition = Predicate.Resolve(c.Predicates);
                    if (c.LockReason !=null || condition)
                    {
                        this.m_Panel.AddChoice(c.Output, condition ? ResourceString.Parse(c.Text) : c.LockReason, condition);
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
                    return null;
                }
                else
                    return this.m_SelectedChoice.StartsWith("#") ? OUTPUT_NEXT : this.m_SelectedChoice;
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
        
    }
}