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
        [XmlAttribute("var")]
        public string Var;
        [XmlAttribute("inc")]
        public string Inc = null;
        [XmlAttribute("dec")]
        public string Dec = null;
        [XmlAttribute("set")]
        public string Set = null;
        [XmlAttribute("icon")]
        public bool ShowIcon = true;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            var console = Resources.Load<Console>(ResourcePaths.SO_CONSOLE);
            var item = Resources.Load<Item>($"{ResourcePaths.SO_ITEMS}/{Name}");
            if (item==null)
                item = Resources.Load<Item>($"{ResourcePaths.SO_ITEMS}/{ResourceString.Parse(Var)}");

            if (item == null)
                console.Log($"ITEM NOT FOUND: '{Name}' / '{Var}'");
            else
            {
                if (Inc != null)
                    controller.Game.AddItem(item, ResourceString.ParseToInt(Inc));
                if (Dec != null)
                    controller.Game.AddItem(item, ResourceString.ParseToInt(Dec) * -1);
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