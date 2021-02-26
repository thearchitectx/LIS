using UnityEngine;
using TheArchitect.Core.Constants;

namespace TheArchitect.Core.Data
{

    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] public string Label;

        public Sprite Icon {
            get { return Resources.Load<Sprite>($"{ResourcePaths.SO_ITEMS}/{this.name}"); }
        }
    }

}
