using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class WaitAction : CutsceneAction
    {
        [XmlAttribute("time")]
        public float Time = 0;
        [XmlAttribute("rnd")]
        public float Rnd = 0;

        private float m_Timer = float.NaN;

        public override void ResetState()
        {
            m_Timer = float.NaN;
        }

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (float.IsNaN(m_Timer))
                m_Timer = Rnd > 0
                    ? UnityEngine.Random.Range(Time-Rnd, Time+Rnd)
                    : Time;

            m_Timer -= UnityEngine.Time.deltaTime;

            #if UNITY_EDITOR
            if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X)) {
                return OUTPUT_NEXT;
            }
            #endif

            return m_Timer <= 0 ? OUTPUT_NEXT : null;
        }

    }
}