using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TheArchitect.Core.Constants;
using TheArchitect.Core.Data.Variables;

namespace TheArchitect.Core.Controllers
{
    public class QuestLog : MonoBehaviour
    {
        
        [SerializeField] private Text m_Text;
        private Game m_Game;

        void Start()
        {
            this.m_Game = Resources.Load<Game>(ResourcePaths.SO_GAME);

            this.m_Game.OnObjectivesUpdate.AddListener(UpdateText);
            this.UpdateText("");
        }

        void OnDestroy()
        {
            this.m_Game.OnObjectivesUpdate.RemoveListener(UpdateText);
        }

        void UpdateText(string updatedQuest)
        {
            string append = "";
            m_Text.text = "";
            foreach (string q in m_Game.State.ObjectiveIndex)
            {
                StringVariable sv = Resources.Load<StringVariable>($"{ResourcePaths.SO_OBJECTIVES}/{q}");
                string objName = sv != null ? sv.Value : q;
                m_Text.text += $"{append}{objName}";
                append = "\n\n";
            }
            if (m_Text.text == "")
                m_Text.text = "NO CURRENT OBJECTIVES";

        }
    }
}