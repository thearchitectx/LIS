using System.Collections.Generic;
using UnityEngine;

namespace TheArchitect.Core.Data.Variables
{
    [CreateAssetMenu(fileName = "Data/Console", menuName = "Console")]
    public class Console : ScriptableObject
    {
        public float DiscardTime;
        [System.NonSerialized] public Sprite ImagePopup;
        [System.NonSerialized] public string DebugMessage;
        [System.NonSerialized] private List<string> m_Messages;

        public IReadOnlyList<string> Messages {
            get { return m_Messages == null ? m_Messages = new List<string>() : m_Messages; }
        }

        public void Log(string message) 
        {
            if (Messages.Count == 0 || Messages[Messages.Count - 1] != message) {
                (Messages as List<string>).Add(message);
            }
        }

        public void PurgeOldest()
        {
            if (Messages.Count > 0)
                (Messages as List<string>).RemoveAt(0);

            ImagePopup = null;
        }

        
    }
}