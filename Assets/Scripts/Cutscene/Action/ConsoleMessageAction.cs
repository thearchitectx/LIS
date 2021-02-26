using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class ConsoleMessageAction : CutsceneAction
    {
        [XmlText]
        public string Message = "Message";

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            var consoleMessages = Resources.Load<Console>(ResourcePaths.SO_CONSOLE);
            consoleMessages.Log(ResourceString.Parse(Message));
            return OUTPUT_NEXT;
        }

    }
}