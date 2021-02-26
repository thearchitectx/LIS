using UnityEngine.UI;
using UnityEngine;
using TheArchitect.Core.Data;

namespace TheArchitect.Controllers.UI.PanelPause
{

    public class PanelStatCategory : MonoBehaviour
    {
        [SerializeField] public GameObject StatPrefab;
        [SerializeField] public GameObject PerkPrefab;
        [SerializeField] public Text TextTitle;

        public void AddStat(string label, string value)
        {
            PanelStat newStat = Instantiate(StatPrefab).GetComponent<PanelStat>();
            newStat.TextLabel.text = label;
            newStat.TextValue.text = value;
            newStat.transform.SetParent(this.transform, false);
        }

        public void AddPerk(Perk perk)
        {
            PanelPerk newPerk = Instantiate(PerkPrefab).GetComponent<PanelPerk>();
            newPerk.TextLabel.text = perk.Label;
            newPerk.TextDescription.text = perk.Description;
            newPerk.ImageIcon.sprite = perk.Icon;
            newPerk.transform.SetParent(this.transform, false);
        }
    }

}
