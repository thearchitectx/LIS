using UnityEngine;
using TheArchitect.Core.Constants;

namespace TheArchitect.Core.Data
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Perk/Flag Based")]
    public class PerkFlag : Perk
    {
        [SerializeField] public string FlagName;
        [SerializeField] public int MinimumValue = 1;
        [SerializeField] public int MaximumValue = int.MaxValue;


        public override bool IsActive(Game game)
        {
            int c = game.GetFlagState(FlagName);
            return c >= MinimumValue && c <= MaximumValue;
        }

    }
}