using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class SystemMessageAction : CutsceneAction
    {
        [XmlText]
        public string Message = "Message";
        
        [XmlIgnore]
        private GameObject m_Canvas;
        private GameObject m_Panel;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (this.m_Canvas == null)
            {
                this.m_Canvas = GameObject.Instantiate(Resources.Load<GameObject>(ResourcePaths.PREFAB_CANVAS_BASE));
                this.m_Canvas.transform.SetParent(controller.transform);
                
                this.m_Panel = GameObject.Instantiate(Resources.Load<GameObject>(ResourcePaths.PREFAB_PANEL_SYSTEM_MESSAGE));
                this.m_Panel.transform.SetParent(this.m_Canvas.transform, false);
                
                UnityEngine.UI.Text text = this.m_Panel.GetComponentInChildren<UnityEngine.UI.Text>();
                if (text != null)
                {
                    text.text = ResourceString.Parse(Message);
                }
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
            {
                this.m_Panel.GetComponent<Animator>().SetBool("shown", false);
                GameObject.Destroy(this.m_Canvas, 1);
                return OUTPUT_NEXT;
            }
            
            return null;
        }

        public override object Valid(CutsceneInstance cutscene)
        {
            return Message != null && Message != "";
        }
    }
}