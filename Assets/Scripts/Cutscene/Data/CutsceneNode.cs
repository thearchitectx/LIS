using System.Xml.Serialization;
using  TheArchitect.Cutscene.Action;

namespace TheArchitect.Cutscene
{
    public class CutsceneNode
    {
        
        public string m_Id;
        private string m_DefaultOutput;
        private CutsceneAction[] m_Actions;
        private int m_CurrentAction = 0;

        [XmlAttribute("id")]
        public string Id 
        {
            set { this.m_Id = value; }
            get { return this.m_Id = this.m_Id != null ? this.m_Id : "node"; }
        }

        [XmlAttribute("out")]
        public string Output 
        {
            set { this.m_DefaultOutput = value; }
            get { return this.m_DefaultOutput; }
        }

        [XmlElement("nop", typeof(CutsceneAction)),
            XmlElement("cutscene-outcome", typeof(OutcomeAction)),
            XmlElement("node-output", typeof(OutputAction)),
            XmlElement("obj", typeof(ObjectAction)),
            XmlElement("prefab", typeof(PrefabAction)),
            XmlElement("wait", typeof(WaitAction)),
            XmlElement("anim", typeof(AnimateAction)),
            XmlElement("sys", typeof(SystemMessageAction)),
            XmlElement("dlg", typeof(DialogAction)),
            XmlElement("choice", typeof(ChoiceAction)),
            XmlElement("stat", typeof(StatAction)),
            XmlElement("rig-track", typeof(RigTrackAction)),
            XmlElement("fade-to-black", typeof(FadeToBlackAction)),
            XmlElement("console", typeof(ConsoleMessageAction)),
            XmlElement("if", typeof(IfAction)),
            XmlElement("load", typeof(StageLoadAction)),
            XmlElement("flag", typeof(SetFlagAction)),
            XmlElement("objective", typeof(ObjectiveAction)),
            XmlElement("item", typeof(ItemAction)),
            XmlElement("text", typeof(SetText)),
            XmlElement("trophy", typeof(TrophyAction)),
            XmlElement("walker", typeof(WalkerAction)),
            XmlElement("sfx", typeof(SFXAction)),
        ]
        public CutsceneAction[] Actions
        {
            set { this.m_Actions = value; }
            get { return this.m_Actions == null ? this.m_Actions = new CutsceneAction[0] : this.m_Actions; }
        }

        [XmlIgnore]
        public CutsceneAction CurrentAction
        {
            get { return m_CurrentAction < this.Actions.Length ? this.Actions[m_CurrentAction] : null; }
        }

        public bool HasNextAction()
        {
            return m_CurrentAction+1 < this.Actions.Length;
        }

        public CutsceneAction NextAction()
        {
            m_CurrentAction++;
            return CurrentAction;
        }

        public void ResetState()
        {
            this.m_CurrentAction = 0;
            foreach (var a in  this.m_Actions)
                a.ResetState();
        }
    }
}