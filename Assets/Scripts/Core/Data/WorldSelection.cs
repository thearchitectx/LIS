using UnityEngine;

namespace TheArchitect.Core.Data
{
    [CreateAssetMenu(menuName = "Data/World Selection")]
    public class WorldSelection : ScriptableObject
    {
        [System.NonSerialized]
        public WorldSelectionItem Selection;
        [System.NonSerialized]
        public WorldSelectionItem Hover;
    }

}