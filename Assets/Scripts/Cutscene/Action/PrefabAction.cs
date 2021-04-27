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
        [XmlAttribute("store")]
        public string Store;
        [XmlAttribute("destroy")]
        public int Destroy = int.MinValue;
        [XmlAttribute("clear")]
        public bool Clear = false;
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

        public override void ResetState()
        {
            this.m_Instance = null;
            this.m_SceneObject = null;
            this.m_Animator = null;
        }

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            if (Destroy!=null) {
                Transform d = controller.FindProxy(Destroy);
                if (d!=null)
                    GameObject.Destroy(d.gameObject);
                else
                    Debug.LogWarning($"Can't find target to destroy: {Destroy}");
            }

            Target = Target == null ? Name : Target;
            if (Target == null && Resource!=null)
            {
                Target = Resource.Contains("/") ? Resource.Substring(Resource.LastIndexOf("/") + 1) : Resource;
            }
            
            if (Target == null)
            {
                if (Destroy == null)
                    Debug.LogWarning($"Can't resolve a target Name: '{Name}'  Resource: '{Resource}'");
                return OUTPUT_NEXT;
            }

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
                        if (string.IsNullOrEmpty(o.Name) || o.Name == this.m_SceneObject.Outcome)
                        {
                            if (o.Destroy > 0)
                                GameObject.Destroy(this.m_SceneObject.gameObject, o.Destroy);

                            if (!string.IsNullOrEmpty(o.Store))
                                controller.Game.SetTextState(o.Store, o.Name);

                            if (o.Clear)
                                this.m_SceneObject.ClearOutcome();

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


/**** ASYNC LOAD
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
        [XmlAttribute("store")]
        public string Store;
        [XmlAttribute("destroy")]
        public int Destroy = int.MinValue;
        [XmlAttribute("clear")]
        public bool Clear = false;
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
        private ResourceRequest m_PrefabRequest = null;
        private SceneObject m_SceneObject = null;

        [XmlIgnore]
        private Animator m_Animator;

        public override void ResetState()
        {
            this.m_Instance = null;
            this.m_PrefabRequest = null;
            this.m_SceneObject = null;
            this.m_Animator = null;
        }

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            #region Async Prefab load management
            if (this.m_PrefabRequest!=null)
            {
                if (this.m_PrefabRequest.isDone)
                {
                    if (this.m_PrefabRequest.asset == null) 
                    {
                        Debug.LogWarning($"Couldn't load {GetPrefabPath()}");
                        return OUTPUT_NEXT;
                    }
                    else
                    {
                        var prefab = this.m_PrefabRequest.asset as GameObject;
                        this.m_PrefabRequest = null;

                        var obj = GameObject.Instantiate(prefab);
                        this.m_Instance = obj.transform;
                        this.m_Instance.name = Target;
                        this.m_Instance.transform.SetParent(Parent == null ? controller.transform : controller.FindProxy(Parent));
                        SetupInstance();
                    }
                }
                else
                {
                    return null;
                }
            }
            #endregion

            #region Destroy command proccessing
            if (Destroy!=null) {
                Transform d = controller.FindProxy(Destroy);
                if (d!=null)
                {
                    GameObject.Destroy(d.gameObject);
                }
                else
                {
                    Debug.LogWarning($"Can't find target to destroy: {Destroy}");
                }
            }
            #endregion

            #region Instance initialization
            if (this.m_Instance == null)
            {
                Target = Target == null ? Name : Target;
                if (Target == null && Resource!=null)
                {
                    Target = Resource.Contains("/") ? Resource.Substring(Resource.LastIndexOf("/") + 1) : Resource;
                }
                
                if (Target == null)
                {
                    Debug.LogWarning($"Can't resolve a target Name: '{Name}'  Resource: '{Resource}'");
                    return OUTPUT_NEXT;
                }

                this.m_Instance = controller.FindProxy(Target);
                if (this.m_Instance==null)
                {
                    this.m_PrefabRequest = Resources.LoadAsync<GameObject>(GetPrefabPath());
                    return null;
                    // GameObject prefab = Resources.Load<GameObject>(prefabPath);
                    // if (prefab == null) 
                    // {
                    //     Debug.LogWarning($"Couldn't load {prefabPath}");
                    //     return OUTPUT_NEXT;
                    // }
                    
                    // var obj = GameObject.Instantiate(prefab);
                    // this.m_Instance = obj.transform;
                    // this.m_Instance.name = Target;
                    // if (Parent == null)
                    //     this.m_Instance.transform.SetParent(controller.transform);
                    // else
                    //     this.m_Instance.transform.SetParent(controller.FindProxy(Parent));
                }
                else
                {
                    SetupInstance();
                }
                // this.m_SceneObject = this.m_Instance.GetComponent<SceneObject>();

                // if (this.Messages!=null)
                //     foreach (var m in this.Messages)
                //     {
                //         object o = m.StringParam != null ? ResourceString.Parse(m.StringParam) : null;
                //         if (o!=null) 
                //             this.m_Instance.SendMessage(m.Name, o, SendMessageOptions.DontRequireReceiver);
                //         else
                //             this.m_Instance.SendMessage(m.Name, SendMessageOptions.DontRequireReceiver);

                //     }
            }
            #endregion
            
            #region Outcome processing
            if (this.Outcomes != null && this.m_SceneObject != null) {
                if (this.m_SceneObject.Outcome != null && this.m_SceneObject.Outcome != "")
                {
                    foreach (var o in Outcomes) 
                        if (o.Name == this.m_SceneObject.Outcome)
                        {
                            if (o.Destroy > 0)
                                GameObject.Destroy(this.m_SceneObject.gameObject, o.Destroy);

                            if (!string.IsNullOrEmpty(o.Store))
                                controller.Game.SetTextState(o.Store, o.Name);

                            if (o.Clear)
                                this.m_SceneObject.ClearOutcome();

                            return o.Output.StartsWith("#") ? OUTPUT_NEXT : o.Output;
                        }

                    return OUTPUT_NEXT;
                }
                return null;
            } else {
                this.m_Instance = null;
                return OUTPUT_NEXT;
            }
            #endregion

        }

        private string GetPrefabPath()
        {
            return this.Resource != null
                ? this.Resource
                : $"{ResourcePaths.FOLDER_PREFABS}/{Name}/{Name}";
        }

        private void SetupInstance()
        {
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

    }
}

****/