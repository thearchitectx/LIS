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
        [XmlText]
        public string Message = null;
        
        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            string resolvedName = Name;
            if (!string.IsNullOrEmpty(Ref))
            {
                resolvedName = controller.Game.GetTextState(Ref);
            }

            if (Inc!=null)
            {
                int v = controller.Game.GetFlagState(resolvedName);
                controller.Game.SetFlagState(resolvedName, v + ResourceString.ParseToInt(Inc));
            }

            if (Set != null)
            {
                controller.Game.SetFlagState(resolvedName, ResourceString.ParseToInt(Set));
            }

            if (Bit0 != null)
            {
                int v = controller.Game.GetFlagState(resolvedName);
                controller.Game.SetFlagState(resolvedName, v & ~(1 << ResourceString.ParseToInt(Bit0)));
            }

            if (Bit1 != null)
            {
                int v = controller.Game.GetFlagState(resolvedName);
                controller.Game.SetFlagState(resolvedName, v | 1 << ResourceString.ParseToInt(Bit1));
            }

            if (Message!=null && Message!="")
            {
                Resources.Load<Console>(ResourcePaths.SO_CONSOLE).Log(
                    ResourceString.Parse(Message)
                );
            }

            return OUTPUT_NEXT;
        }

    }
}