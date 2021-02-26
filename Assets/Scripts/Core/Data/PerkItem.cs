using UnityEngine;
using TheArchitect.Core.Constants;

namespace TheArchitect.Core.Data
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Perk/Item Based")]
    public class PerkItem : Perk
    {
        [SerializeField] public Item item;
        [SerializeField] public int MinimumAmount = 1;
        [SerializeField] public int MaximumCount = int.MaxValue;


        public override bool IsActive(Game game)
        {
            int c = game.GetItemCount(item.name);
            return c >= MinimumAmount && c <= MaximumCount;
        }

    }
}