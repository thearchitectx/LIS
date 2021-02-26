using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Data;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Core.Constants;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class ItemAction : CutsceneAction
    {
        [XmlText]
        public string Message;
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("inc")]
        public string Inc = null;
        [XmlAttribute("set")]
        public string Set = null;
        [XmlAttribute("icon")]
        public bool ShowIcon = true;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            var console = Resources.Load<Console>(ResourcePaths.SO_CONSOLE);
            var item = Resources.Load<Item>($"{ResourcePaths.SO_ITEMS}/{Name}");

            if (item == null)
                console.Log($"ITEM NOT FOUND: '{Name}'");
            else
            {
                if (Inc != null)
                    controller.Game.AddItem(item, ResourceString.ParseToInt(Inc));
                if (Set != null)
                    controller.Game.SetItem(item, ResourceString.ParseToInt(Set));

                if (Message != null && Message != "")
                    console.Log(ResourceString.Parse(Message));

                if (ShowIcon)
                    console.ImagePopup = item.Icon;
            }

            return OUTPUT_NEXT;
        }

    }
}