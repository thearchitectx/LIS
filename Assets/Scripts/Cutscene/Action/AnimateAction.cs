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
        [XmlAttribute("idle")]
        public CharacterIdle Idle = CharacterIdle.NONE;
        [XmlAttribute("react")]
        public CharacterReact React = CharacterReact.NONE;
        [XmlAttribute("expression")]
        public CharacterExpression Expression = CharacterExpression.NONE;
        [XmlAttribute("float")]
        public string Float = null;
        [XmlAttribute("bool")]
        public string Bool = null;
        [XmlAttribute("intValue")]
        public int IntValue = int.MinValue;
        [XmlAttribute("floatValue")]
        public float FloatValue = float.MinValue;
        [XmlAttribute("boolValue")]
        public bool BoolValue = false;
        [XmlAttribute("sync")]
        public int LayerToWait = int.MinValue;
        [XmlAttribute("target")]
        public string Target = null;
        [XmlAttribute("context")]
        public string Context = null;

        [XmlIgnore]
        private Animator m_Animator;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (this.m_Animator == null)
            {
                Transform t = null;
                if (string.IsNullOrEmpty(Context))
                    t = string.IsNullOrEmpty(this.Target)
                        ? controller.transform
                        : controller.FindProxy(Target);
                else
                    t = string.IsNullOrEmpty(this.Target)
                        ? controller.FindProxy(Context)
                        : controller.FindProxy(Context)?.Find(Target);

                if (t==null)
                {
                    Debug.LogWarning($"Can't find target - CONTEXT:'{Context}' TARGET:'{Target}'");
                    return OUTPUT_NEXT;
                }

                this.m_Animator = t.GetComponent<Animator>();

                if (this.m_Animator==null)
                {
                    Debug.LogWarning($"Can't find animator on target - CONTEXT:'{Context}' TARGET:'{Target}'");
                    return OUTPUT_NEXT;
                }

                if (Idle != CharacterIdle.NONE)
                    this.m_Animator.SetInteger("idle", (int) Idle);

                if (Expression != CharacterExpression.NONE)
                    this.m_Animator.SetInteger("exp", (int) Expression);

                if (React != CharacterReact.NONE)
                    this.m_Animator.SetTrigger($"REACT_{React.ToString()}");
                    
                if (Int != null)
                    this.m_Animator.SetInteger(Int, IntValue);

                if (Float != null)
                    this.m_Animator.SetFloat(Float, FloatValue);
                
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

        public enum CharacterIdle
        {
            NONE,
            STAND_HANDS_WAIST,
            UNEASY,
            POSE_PRAY,
            POSE_CHEER,
            UPSET,
        }

        public enum CharacterReact
        {
            NONE,
            FACEPALM,
            SUSPICIOUS,
            SURPRISE,
            LEAN_THINK,
            YAY,
            SAY01
        }

        public enum CharacterExpression
        {
            NONE,
            UNEASY,
            DISAPPROVE,
            NEUTRAL,
            SMILE_CHEER,
        }
    }

    
}