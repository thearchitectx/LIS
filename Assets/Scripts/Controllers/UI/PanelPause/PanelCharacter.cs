using UnityEngine.UI;
using UnityEngine;
using TheArchitect.Core.Constants;
using TheArchitect.Core;
using TheArchitect.Core.Data;

namespace TheArchitect.Controllers.UI.PanelPause
{

    public class PanelCharacter : MonoBehaviour
    {
        [SerializeField] public GameObject PanelStatPrefab;
        [SerializeField] public Image ImageCharacter;
        [SerializeField] public Text TextProfile;
        [SerializeField] public Text TextIntel;
        [SerializeField] public Transform PanelStats;

        public void SetCharacter(Game game, Character character)
        {
            this.ImageCharacter.sprite = character.Sprite;
            this.ImageCharacter.color = Color.white;

            foreach (Transform t in this.PanelStats.transform)
                Destroy(t.gameObject);
            
            this.TextIntel.text = "";
            for (int i=0; i < character.Intel.Length; i++)
            {
                this.TextIntel.text += game.GetCharacterStat(character, string.Format(Character.STAT_INTEL_, i)) > 0
                    ? $"- {character.Intel[i]}\n\n"
                    : "- ????\n\n";
            }

            this.TextProfile.text = $"<color=cyan>AGE</color>\n{character.Age}\n\n<color=cyan>STATUS</color>\nACQUAINTANCE";

            AddStat(game, character, Character.STAT_AFFINITY, "AFFINITY", true);
            AddStat(game, character, Character.STAT_CORRUPTION, "CORRUPTION", false);
        }

        private void AddStat(Game game, Character character, string stat, string label, bool force)
        {
            int s = game.GetCharacterStat(character, stat);
            if ( s != 0 || force)
            {
                PanelStat panelStat = Instantiate(PanelStatPrefab).GetComponent<PanelStat>();
                panelStat.transform.SetParent(PanelStats, false);
                panelStat.TextLabel.text = label;
                panelStat.TextValue.text = s.ToString("D2");
            }
        }

    }

}
