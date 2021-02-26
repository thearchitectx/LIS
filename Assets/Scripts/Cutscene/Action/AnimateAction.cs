using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class AnimateAction : CutsceneAction
    {
        [XmlAttribute("trigger")]
        public string Trigger = null;
        [XmlAttribute("int")]
        public string Int = null;
        [XmlAttribute("bool")]
        public string Bool = null;
        [XmlAttribute("intValue")]
        public int IntValue = int.MinValue;
        [XmlAttribute("boolValue")]
        public bool BoolValue = false;
        [XmlAttribute("sync")]
        public int LayerToWait = int.MinValue;
        [XmlAttribute("target")]
        public string Target = null;

        [XmlIgnore]
        private Animator m_Animator;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (this.m_Animator == null)
            {
                this.m_Animator = this.Target == null
                    ? controller.GetComponent<Animator>()
                    : controller.FindProxy(Target).GetComponent<Animator>();
                    
                if (Int != null)
                    this.m_Animator.SetInteger(Int, IntValue);
                
                if (Bool != null)
                    this.m_Animator.SetBool(Bool, BoolValue);

                if (Trigger != null)
                    this.m_Animator.SetTrigger(Trigger);

                return null;
            }

            if (this.LayerToWait > int.MinValue && this.m_Animator.IsInTransition(LayerToWait))
            {
                return null;
            }

            return OUTPUT_NEXT;
        }

        public override object Valid(CutsceneInstance cutscene)
        {
            return (Int != null && IntValue > int.MinValue) || Trigger != null || Bool != null;
        }
    }
}