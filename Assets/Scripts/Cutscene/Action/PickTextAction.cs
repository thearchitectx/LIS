using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{

    public class PickTextAction : CutsceneAction
    {
        [XmlAttribute("text")]
        public string Text;
        [XmlElement("t")]
        public string[] Options;
        [XmlElement("flag", typeof(ByFlagValueRepeat))]
        public PickTextRule Rule;
        
        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            var s = Rule.Pick(cutscene, controller, Options);
            controller.Game.SetTextState(Text, s);
            return OUTPUT_NEXT;
        }

    }

    public abstract class PickTextRule 
    {
        public abstract string Pick(CutsceneInstance cutscene, CutsceneController controller, string[] Options);
    }

    public class ByFlagValueRepeat  : PickTextRule
    {
        [XmlAttribute("name")]
        public string Flag;

        public override string Pick(CutsceneInstance cutscene, CutsceneController controller, string[] Options)
        {
            var f = controller.Game.GetFlagState(Flag);
            return Options[f % Options.Length];
        }
    }
}