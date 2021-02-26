using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class OutputAction : CutsceneAction
    {
        [XmlAttribute("node")]
        public string Node = null;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            return Node;
        }

    }
}