using UnityEngine;
using TheArchitect.Core.Constants;

namespace TheArchitect.Core.Data
{
    public class Perk : ScriptableObject
    {
        [SerializeField] public string Label;
        [SerializeField] public string Description;

        public virtual bool IsActive(Game game)
        {
            return false;
        }
        
        public Sprite Icon {
            get { return Resources.Load<Sprite>($"{ResourcePaths.SO_PERKS}/{this.name}"); }
        }
    }
    
}