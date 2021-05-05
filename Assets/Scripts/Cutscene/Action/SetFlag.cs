using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{

    public class SetFlagAction : CutsceneAction
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("ref")]
        public string Ref;
        [XmlAttribute("inc")]
        public string Inc = null;
        [XmlAttribute("set")]
        public string Set = null;
        [XmlAttribute("bit-set")]
        public string Bit1 = null;
        [XmlAttribute("bit-unset")]
        public string Bit0 = null;
        [XmlAttribute("random-max")]
        public string Random = null;
        [XmlText]
        public string Message = null;
        
        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            string resolvedName = Name;
            if (!string.IsNullOrEmpty(Ref))
            {
                resolvedName = controller.Game.GetTextState(Ref);
            }

            if (!string.IsNullOrEmpty(Inc))
            {
                int v = controller.Game.GetFlagState(resolvedName);
                controller.Game.SetFlagState(resolvedName, v + ResourceString.ParseToInt(Inc));
            }

            if (!string.IsNullOrEmpty(Set ))
            {
                controller.Game.SetFlagState(resolvedName, ResourceString.ParseToInt(Set));
            }

            if (!string.IsNullOrEmpty(Bit0 ))
            {
                int v = controller.Game.GetFlagState(resolvedName);
                controller.Game.SetFlagState(resolvedName, v & ~(1 << ResourceString.ParseToInt(Bit0)));
            }

            if (!string.IsNullOrEmpty(Bit1 ))
            {
                int v = controller.Game.GetFlagState(resolvedName);
                controller.Game.SetFlagState(resolvedName, v | 1 << ResourceString.ParseToInt(Bit1));
            }

            if (!string.IsNullOrEmpty(Random))
            {
                int max = ResourceString.ParseToInt(Random);
                int v = UnityEngine.Random.Range(1, max+1);
                controller.Game.SetFlagState(resolvedName, v);
            }

            if (!string.IsNullOrEmpty(Message))
            {
                Resources.Load<Console>(ResourcePaths.SO_CONSOLE).Log(
                    ResourceString.Parse(Message)
                );
            }

            return OUTPUT_NEXT;
        }

    }
}