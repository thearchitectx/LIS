using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;
using TheArchitect.Core.Constants;
using TheArchitect.Core;
using TheArchitect.SceneObjects;

namespace TheArchitect.Cutscene.Action
{
    public class PrefabMessage
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("string")]
        public string StringParam;
    }
    
    public class PrefabOutcome
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("output")]
        public string Output;
        [XmlAttribute("destroy")]
        public int Destroy = int.MinValue;
    }

    public class PrefabAction : CutsceneAction
    {
        [XmlAttribute("destroy")]
        public string Destroy = null;
        [XmlAttribute("name")]
        public string Name = null;
        [XmlAttribute("resource")]
        public string Resource = null;
        [XmlAttribute("parent")]
        public string Parent = null;
        [XmlAttribute("target")]
        public string Target = null;
        [XmlElement("outcome", typeof(PrefabOutcome))]
        public PrefabOutcome[] Outcomes = null;
        [XmlElement("message", typeof(PrefabMessage))]
        public PrefabMessage[] Messages = null;

        private Transform m_Instance = null;
        private SceneObject m_SceneObject = null;

        [XmlIgnore]
        private Animator m_Animator;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (Destroy!=null) {
                Transform d = controller.FindProxy(Destroy);
                if (d!=null)
                    GameObject.Destroy(d.gameObject);
            }

            Target = Target == null ? Name : Target;
            if (Target == null)
                return OUTPUT_NEXT;

            if (this.m_Instance == null)
            {
                this.m_Instance = controller.FindProxy(Target);
                if (this.m_Instance==null)
                {
                    var prefabPath = this.Resource != null
                        ? this.Resource
                        : $"{ResourcePaths.FOLDER_PREFABS}/{Name}/{Name}";

                    GameObject prefab = Resources.Load<GameObject>(prefabPath);
                    if (prefab == null) 
                    {
                        Debug.LogWarning($"Couldn't load {prefabPath}");
                        return OUTPUT_NEXT;
                    }
                    
                    var obj = GameObject.Instantiate(prefab);
                    this.m_Instance = obj.transform;
                    this.m_Instance.name = Target;
                    if (Parent == null)
                        this.m_Instance.transform.SetParent(controller.transform);
                    else
                        this.m_Instance.transform.SetParent(controller.FindProxy(Parent));
                }
                this.m_SceneObject = this.m_Instance.GetComponent<SceneObject>();

                if (this.Messages!=null)
                    foreach (var m in this.Messages)
                    {
                        object o = m.StringParam != null ? ResourceString.Parse(m.StringParam) : null;
                        if (o!=null) 
                            this.m_Instance.SendMessage(m.Name, o, SendMessageOptions.DontRequireReceiver);
                        else
                            this.m_Instance.SendMessage(m.Name, SendMessageOptions.DontRequireReceiver);

                    }
            }
            
            if (this.Outcomes != null && this.m_SceneObject != null) {
                if (this.m_SceneObject.Outcome != null && this.m_SceneObject.Outcome != "")
                {
                    foreach (var o in Outcomes) 
                        if (o.Name == this.m_SceneObject.Outcome)
                        {
                            if (o.Destroy > 0) GameObject.Destroy(this.m_SceneObject.gameObject, o.Destroy);
                            return o.Output.StartsWith("#") ? OUTPUT_NEXT : o.Output;
                        }

                    return OUTPUT_NEXT;
                }
                return null;
            } else {
                this.m_Instance = null;
                return OUTPUT_NEXT;
            }

        }

    }
}