using UnityEngine;
using UnityEngine.Events;
using TheArchitect.Core;
using TheArchitect.Cutscene.Data;
using TheArchitect.Cutscene.Action;

namespace TheArchitect.Cutscene
{
    public class CutsceneController : MonoBehaviour
    {
        
        [SerializeField] public Game Game;
        [SerializeField] private CutsceneContext m_Context;
        [SerializeField] [TextArea] private string m_ScriptPath;
        [SerializeField] private CutsceneProxy[] m_Proxies;
        [SerializeField] private CutsceneParam[] m_TextParams;

        private CutsceneInstance m_Cutscene;
        public UnityEvent<string> OnFinished = new UnityEvent<string>();

        public void Reload()
        {
            this.Game.DisablePlayer = false;
            if (this.m_TextParams!=null)
                foreach (var p in this.m_TextParams)
                    Game.SetTextState(p.Name, p.Value);

            this.m_Cutscene = CutsceneLoader.Load(m_Context, m_ScriptPath);
            this.OnFinished.RemoveAllListeners();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (this.m_Cutscene==null)
            {
                Reload();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (this.m_Cutscene != null)
            {
                if (this.m_Cutscene.Finished)
                {
                    Game.AllowSaving = true;
                    if (this.m_TextParams!=null)
                        foreach (var p in this.m_TextParams)
                            Game.SetTextState(p.Name, null);
                    
                    this.OnFinished.Invoke(this.m_Cutscene.Outcome);
                    this.OnFinished.RemoveAllListeners();

                    if (this.m_Cutscene.Outcome == CutsceneAction.OUTCOME_DESTROY_OBJECT) 
                        Destroy(this.gameObject, 1);
                    else if (this.m_Cutscene.Outcome == CutsceneAction.OUTCOME_DESTROY_CONTROLLER)
                        Destroy(this, 1);
                    this.m_Cutscene = null;
                } else {
                    Game.AllowSaving = false;
                    this.m_Cutscene.Update(this);
                }
            }
            
        }

        public Transform FindProxy(string name)
        {
            if (name == null)
                return null;

            if (name==".")
                return this.transform;

            foreach (var p in this.m_Proxies)
            {
                if (p.Name == name)
                {
                    return p.Transform;
                }
            }

            if (name.StartsWith("/"))
                return GameObject.Find(name.Substring(1))?.transform;

            if (name.StartsWith("../"))
                return transform.parent.Find(name.Substring(3))?.transform;

            try
            {
                return GameObject.FindWithTag(name).transform;
            } 
            catch (System.Exception) 
            {
                return this.transform.Find(name);
            }

        }

    }

    [System.Serializable]
    public class CutsceneProxy
    {
        public string Name;
        public Transform Transform;
    }

    [System.Serializable]
    public class CutsceneParam
    {
        public string Name;
        [TextArea] public string Value;
    }

}

