using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class WaitAction : CutsceneAction
    {
        [XmlAttribute("time")]
        public float Time = 0;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            Time -= UnityEngine.Time.deltaTime;
            #if UNITY_EDITOR
            if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X)) {
                return OUTPUT_NEXT;
            }
            #endif

            return Time <= 0 ? OUTPUT_NEXT : null;
        }

        public override object Valid(CutsceneInstance cutscene)
        {
            return Time > 0;
        }
    }
}