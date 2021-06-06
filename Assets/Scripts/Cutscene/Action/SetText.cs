using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{

    public class SetText : CutsceneAction
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("ref")]
        public bool IsNameRef = false;
        [XmlAttribute("set")]
        public string Set = null;
        
        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            string n = IsNameRef ? controller.Game.GetTextState(Name) : Name;
            controller.Game.SetTextState(n, ResourceString.Parse(Set) );
            return OUTPUT_NEXT;
        }

    }
}