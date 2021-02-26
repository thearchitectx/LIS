using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Data;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{
    public class StatAction : CutsceneAction
    {
        [XmlAttribute("char")]
        public string Character;
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("set")]
        public int Set = int.MinValue;
        [XmlAttribute("inc")]
        public int Inc = int.MinValue;
        [XmlAttribute("notify")]
        public bool Notify = true;
        [XmlAttribute("clamp-a")]
        public int ClampA = int.MinValue;
        [XmlAttribute("clamp-b")]
        public int ClampB = int.MaxValue;
        [XmlText]
        public string Message = null;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            Character character = Resources.Load<Character>($"{ResourcePaths.SO_CHARACTERS}/{this.Character}");
            int origValue = controller.Game.GetCharacterStat(character, Name);

            if (Inc > int.MinValue)
            {
                controller.Game.SetCharacterStat(character, Name, Mathf.Clamp(origValue+Inc, ClampA, ClampB) );
            }

            if (Set > int.MinValue)
            {
                controller.Game.SetCharacterStat(character, Name, Mathf.Clamp(Set, ClampA, ClampB) );
            }
            

            if (Notify && origValue != controller.Game.GetCharacterStat(character, Name))
            {
                Message = Message != null ? Message : $"That action affected <color=cyan>{character.DisplayName}</color>";
                var consoleMessages = Resources.Load<Console>(ResourcePaths.SO_CONSOLE);
                consoleMessages.Log(ResourceString.Parse(Message).ToUpper());
            }

            return OUTPUT_NEXT;
        }

        public override object Valid(CutsceneInstance cutscene)
        {
            Character character = Resources.Load<Character>($"{ResourcePaths.SO_CHARACTERS}/{this.Character}");
            return character != null && this.Name != null && this.Set != 0;
        }
    }
}