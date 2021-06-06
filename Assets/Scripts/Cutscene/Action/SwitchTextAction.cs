using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{

    public class SwitchTextAction : CutsceneAction
    {
        [XmlAttribute("check")]
        public string Check;
        [XmlAttribute("text")]
        public string Text;
        [XmlElement("case")]
        public SwitchCase[] Cases;
        
        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            var value = ResourceString.Parse(Check);
            foreach (var c in Cases)
            {
                #if UNITY_EDITOR
                Debug.Log($"== '{c.Eq}' '{value}'");
                #endif
                if (c.Eq == value || c.Eq.StartsWith("#"))
                {
                    controller.Game.SetTextState(Text, ResourceString.Parse(c.Then));
                    return OUTPUT_NEXT;
                }
            }

            controller.Game.SetTextState(Text, null);
            return OUTPUT_NEXT;
        }

    }

    public class SwitchCase 
    {
        [XmlAttribute("eq")]
        public string Eq;
        [XmlText]
        public string Then;
    }
}