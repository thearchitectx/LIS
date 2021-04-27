using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;
using TheArchitect.Core;

namespace TheArchitect.Cutscene.Action
{
    public class OutputAction : CutsceneAction
    {
        [XmlAttribute("node")]
        public string Node = null;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            return ResourceString.Parse(Node);
        }

    }
}