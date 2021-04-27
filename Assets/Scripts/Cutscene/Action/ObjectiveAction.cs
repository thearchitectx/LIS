using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{

    public class ObjectiveAction : CutsceneAction
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("completed")]
        public bool Completed = true;
        
        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            bool before = controller.Game.HasObjective(Name);

            if (Completed)
                controller.Game.RemoveObjective(Name);
            else
                controller.Game.AddObjective(Name);
            
            bool after  = controller.Game.HasObjective(Name);

            if (before != after)
            {
                var consoleMessages = Resources.Load<Console>(ResourcePaths.SO_CONSOLE);
                consoleMessages.Log( Completed ? "Objective Completed!" : "New Objective!" );
            }

            return OUTPUT_NEXT;
        }

    }
}