using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class WalkerAction : CutsceneAction
    {
        [XmlAttribute("target")]
        public string Target = null;
        [XmlAttribute("destination")]
        public string Destination = null;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            var t = controller.FindProxy(Target);
            if (t == null)
            {
                Debug.LogWarning($"Can't find walker target '{Target}'");
                return OUTPUT_NEXT;
            }

            var walker = t.GetComponent<SimpleWalkController>();
            if (walker == null)
            {
                Debug.LogWarning($"Can't find a walker controller on '{Target}'");
                return OUTPUT_NEXT;
            }

            var dest = controller.FindProxy(Destination);
            if (dest == null)
            {
                Debug.LogWarning($"Can't find destination '{Destination}'");
            }

            walker.Destination = dest;
            return OUTPUT_NEXT;
        }
    }
    
}