using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class StageLoadAction : CutsceneAction
    {
        [XmlAttribute("stage")]
        public string Stage = null;
        [XmlAttribute("spawn")]
        public string Spawn = null;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (Spawn!=null && Spawn!="")
            {
                controller.Game.SetTextState("TEXT_SPAWN_POINT", Spawn);
            }
            controller.Game.LoadStage(controller.Game.GetStage(Stage));
            return OUTPUT_NEXT;
        }

    }
}