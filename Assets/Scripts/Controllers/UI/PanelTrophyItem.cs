using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core.Data;

namespace TheArchitect.Core.Controllers
{
        
    public class PanelTrophyItem : MonoBehaviour
    {
        
        [SerializeField] public Image ImageBG;
        [SerializeField] public Image ImageTrophy;
        [SerializeField] public Text TextLabel;
        [SerializeField] public Text TextDescription;
        [SerializeField] public Text TextUnlock;
        [SerializeField] public Material MaterialLockedImage;

        public void SetTrophy(Trophy trophy)
        {
            TextLabel.text = trophy.Label;
            TextLabel.color = trophy.Unlocked ? Color.white : Color.gray;
            TextDescription.text = trophy.Description;
            TextDescription.color = trophy.Unlocked ? Color.white : Color.gray;
            TextUnlock.text = trophy.Unlocked ? $"Unlocked {trophy.GetUnlockDate()}" : "";
            
            ImageTrophy.sprite = trophy.RelatedCharacter.Sprite;
            if (!trophy.Unlocked)
            {
                ImageTrophy.material = MaterialLockedImage;
            }

            ImageBG.color = trophy.Unlocked ? new Color32(0x00, 0x63, 0x5D, 0xFF) : new Color32(0x11, 0x11, 0x11, 0xFF);
        }

    }

}