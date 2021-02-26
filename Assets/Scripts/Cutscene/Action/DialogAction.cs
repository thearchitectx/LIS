using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Controllers;
using TheArchitect.Core.Data;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public struct DialogMessage
    {
        [XmlAttribute("wait")]
        public float Wait;
        [XmlText]
        public string Text;
    }

    public class DialogAction : CutsceneAction
    {
        [XmlElement("m")] public DialogMessage[] Messages = null;
        [XmlAttribute("char")] public string CharacterId = null;
        [XmlAttribute("track")] public string Track = null;
        [XmlAttribute("style")] public DialogStyle Style = DialogStyle.DEFAULT;
        [XmlAttribute("jawAnim")] public bool m_JawAnim = true;
        
        [XmlIgnore] private GameObject m_Canvas;
        [XmlIgnore] private PanelDialogObjective m_Panel;
        [XmlIgnore] private int m_CurrentMessage = 0;
        [XmlIgnore] private Character m_Character = null;
        [XmlIgnore] private RigWeightDamper m_JawRig = null;
        [XmlIgnore] private RigWeightDamper m_LipsRig = null;

        private float m_WaitTimer = 0;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (this.m_WaitTimer > 0)
            {
                this.m_WaitTimer -= Time.deltaTime;
                return null;
            }

            if (this.m_CurrentMessage >= this.Messages.Length)
            {
                DestroyDialogPanel();
                m_CurrentMessage = 0;
                return OUTPUT_NEXT;
            }

            if (m_Canvas == null)
            {
                this.m_Character = Resources.Load<Character>($"{ResourcePaths.SO_CHARACTERS}/{CharacterId}");
                Transform characterTransform = controller.FindProxy(CharacterId);

                if (m_JawAnim && characterTransform!=null)
                {
                    var jawRigTransform = characterTransform.Find(SkeletonPaths.RIG_JAW_TALK);
                    var lipsRigTransform = characterTransform.Find(SkeletonPaths.RIG_LIPS_TALK);
                    if (jawRigTransform != null)
                        this.m_JawRig = jawRigTransform.GetComponent<RigWeightDamper>();
                    if (lipsRigTransform != null)
                       this.m_LipsRig = lipsRigTransform.GetComponent<RigWeightDamper>();
                }

                this.m_Canvas = GameObject.Instantiate(Resources.Load<GameObject>(ResourcePaths.PREFAB_CANVAS_BASE));
                this.m_Canvas.transform.SetParent(controller.transform);
                
                this.m_Panel = GameObject.Instantiate(
                        Resources.Load<GameObject>(
                            ResourcePaths.PREFAB_PANEL_DIALOG_OBJECTIVE
                        )
                    ).GetComponent<PanelDialogObjective>();

                this.m_Panel.transform.SetParent(this.m_Canvas.transform, false);
                
                if (this.Track != null && characterTransform!=null)
                {
                    Transform trackedTransform = characterTransform.Find(SkeletonPaths.GetPathTo(this.Track));

                    this.m_Panel.TrackedTransform = trackedTransform;
                }

                this.m_Panel.Display(
                    this.m_Character,
                    ResourceString.Parse(this.Messages[this.m_CurrentMessage].Text),
                    this.Style
                );
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
            {
                if (this.m_Panel.HasRunningDisplay())
                {
                    this.m_Panel.SkipDisplay();
                }
                else if (this.Messages[this.m_CurrentMessage].Wait > 0)
                {
                    DestroyDialogPanel();
                    this.m_WaitTimer = this.Messages[this.m_CurrentMessage].Wait;
                    this.m_CurrentMessage++;
                }
                else if (this.Messages.Length > this.m_CurrentMessage + 1)
                {
                    this.m_Panel.Display(
                        this.m_Character,
                        ResourceString.Parse(this.Messages[++this.m_CurrentMessage].Text),
                        this.Style
                    );
                }
                else
                {
                    DestroyDialogPanel();
                    m_CurrentMessage = 0;
                    return OUTPUT_NEXT;
                }
            }
            else

            // Randomize Jaw Rig to simulate speech
            if (this.m_Panel!=null && this.m_Panel.IsRollingText && this.m_JawRig != null && !this.m_JawRig.IsMoving())
                this.m_JawRig.SetWeight(
                    this.m_JawRig.GetRigWeight() > 0.25f
                        ? UnityEngine.Random.Range(0f, 0.25f)
                        : UnityEngine.Random.Range(0.25f, 1)
                );
            if (this.m_Panel!=null && this.m_Panel.IsRollingText && this.m_LipsRig != null && !this.m_LipsRig.IsMoving())
                this.m_LipsRig.SetWeight(
                    this.m_LipsRig.GetRigWeight() > 0.25f
                        ? UnityEngine.Random.Range(0f, 0.25f)
                        : UnityEngine.Random.Range(0.25f, 1)
                );

            if (this.m_Panel!=null && !this.m_Panel.IsRollingText && this.m_JawRig != null)
                this.m_JawRig.SetWeight(0);
            if (this.m_Panel!=null && !this.m_Panel.IsRollingText && this.m_LipsRig != null)
                this.m_LipsRig.SetWeight(0);
            
            return null;
        }

        private void DestroyDialogPanel()
        {
            if (m_Canvas!=null)
            {
                GameObject.Destroy(m_Canvas);
                m_Canvas = null;
            }
            if (this.m_JawRig != null)
                this.m_JawRig.SetWeight(0);
            if (this.m_LipsRig != null)
                this.m_LipsRig.SetWeight(0);
        }

    }
}