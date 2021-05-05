using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core.Data;
using TheArchitect.Core;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Core.Constants;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class StudioTokenAction : CutsceneAction
    {
        [XmlAttribute("char")]
        public string CharacterId;
        [XmlAttribute("category")]
        public string Category;
        [XmlText]
        public string Message = null;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            GalleryController.Unlock(CharacterId, Category);
            if (!string.IsNullOrEmpty(Message))
            {
                var chr  = Resources.Load<Character>($"{ResourcePaths.SO_CHARACTERS}/{CharacterId}");
                var consoleMessages = Resources.Load<Console>(ResourcePaths.SO_CONSOLE);
                consoleMessages.Log(ResourceString.Parse(Message));
                consoleMessages.ImagePopup = chr.Sprite;
            }
            return OUTPUT_NEXT;
        }

    }
}