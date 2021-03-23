using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core.Data;
using TheArchitect.Core.Constants;
using TheArchitect.Cutscene.Data;
using TheArchitect.Core.Controllers;

namespace TheArchitect.Cutscene.Action
{
    public class TrophyAction : CutsceneAction
    {
        [XmlAttribute("unlock")]
        public string Name;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            var trophy = Resources.Load<Trophy>($"{ResourcePaths.SO_TROPHIES}/{Name}");
            if (trophy.Unlock())
            {
                var request = Resources.LoadAsync<GameObject>($"{ResourcePaths.PREFAB_TROPHY_NOTIFICATION}");
                request.completed += (operation) => {
                    GameObject notification = GameObject.Instantiate((GameObject) request.asset);
                    PanelTrophyItem panel = notification.GetComponentInChildren<PanelTrophyItem>();
                    panel.SetTrophy(trophy);
                    panel.ImageBG.sprite = null;
                    panel.ImageBG.color = new Color32(0x3d, 0x3d, 0x3d, 0xFF);
                };
            }

            return OUTPUT_NEXT;
        }

    }
}