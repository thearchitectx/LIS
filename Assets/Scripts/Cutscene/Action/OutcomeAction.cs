using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class OutcomeAction : CutsceneAction
    {
        [XmlAttribute("value")]
        public string Value = null;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            cutscene.Outcome = Value;
            return OUTPUT_NEXT;
        }

    }
}