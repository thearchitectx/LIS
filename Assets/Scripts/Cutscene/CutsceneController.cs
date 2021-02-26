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
        [SerializeField] private string m_ScriptPath;
        [SerializeField] private CutsceneProxy[] m_Proxies;

        private CutsceneInstance m_Cutscene;
        public UnityEvent<string> OnFinished = new UnityEvent<string>();

        public void Reload()
        {
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
            foreach (var p in this.m_Proxies)
            {
                if (p.Name == name)
                {
                    return p.Transform;
                }
            }
            return this.transform.Find(name);
        }

    }

    [System.Serializable]
    public class CutsceneProxy
    {
        public string Name;
        public Transform Transform;
    }

}

