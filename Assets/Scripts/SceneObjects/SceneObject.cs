using UnityEngine;

namespace TheArchitect.SceneObjects
{

    public class SceneObject : MonoBehaviour
    {
        public string Outcome;

        public void SetOutcome(string o)
        {
            this.Outcome = o;
        }
        
        public void ClearOutcome()
        {
            Outcome = null;
        }
    }
    
}