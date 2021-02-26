using System.Collections.Generic;
using UnityEngine;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Data;
using TheArchitect.Core;

namespace TheArchitect.Controllers.UI.PanelPause
{

    public class PanelItems : MonoBehaviour
    {
        [SerializeField] public GameObject ItemPrefab;

        void Start()
        {
            foreach (Transform t in this.transform)
                Destroy(t.gameObject);
                
            Game game = Resources.Load<Game>(ResourcePaths.SO_GAME);
            foreach (KeyValuePair<Item, int> item in game.GetInventory())
            {
                PanelItem pi =  Instantiate(ItemPrefab).GetComponent<PanelItem>();
                pi.TextLabel.text = item.Key.Label;
                pi.TextValue.text = $"x{item.Value}";
                pi.ImageIcon.sprite = item.Key.Icon;
                pi.ImageIcon.color = Color.white;
                pi.transform.SetParent(this.transform, false);
            }
        }
    }

}
