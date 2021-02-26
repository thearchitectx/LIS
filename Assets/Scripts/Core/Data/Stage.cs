using UnityEngine;

namespace TheArchitect.Core.Data
{

    [CreateAssetMenu(fileName = "New Quest", menuName = "Data/Stage")]
    public class Stage : ScriptableObject
    {
        [SerializeField] public BakedScene Scene;
        [SerializeField] public string MechanicPrefab;
    }

}
