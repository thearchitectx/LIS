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
        [XmlAttribute("inc")]
        public string Inc = null;
        [XmlAttribute("set")]
        public string Set = null;
        [XmlText]
        public string Message = null;
        
        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (Inc!=null)
            {
                int v = controller.Game.GetFlagState(Name);
                controller.Game.SetFlagState(Name, v + ResourceString.ParseToInt(Inc));
            }

            if (Set != null)
            {
                controller.Game.SetFlagState(Name, ResourceString.ParseToInt(Set));
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