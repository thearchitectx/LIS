using System;
using UnityEngine;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class CutsceneAction
    {
        public const string OUTCOME_DESTROY_OBJECT = "_destroyObject";
        public const string OUTCOME_DESTROY_CONTROLLER = "_destroyController";
        public const string OUTPUT_END = "_end";
        public const string OUTPUT_NEXT = "_next";
        
        public virtual void ResetState()
        {
            
        }

        public virtual string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            return OUTPUT_NEXT;
        }

        public virtual object Valid(CutsceneInstance cutscene)
        {
            return null;
        }

        protected virtual void LogIf(bool exp, string msg)
        {
            if (exp)
            {
                Debug.Log($"{this.GetType().Name}: {msg}");
            }
        }

    }
}