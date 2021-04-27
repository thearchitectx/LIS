using UnityEngine;
using System;
using TheArchitect.Core.Constants;

namespace TheArchitect.Core.Data
{

    [CreateAssetMenu(fileName = "New Trophy", menuName = "Data/Trophy")]
    public class Trophy : ScriptableObject
    {
        [SerializeField] private string m_Label;
        [SerializeField] private int m_Order;
        [SerializeField] [TextArea] private string m_Description;
        [SerializeField] private Character m_RelatedCharacter;

        public int Order { get { return m_Order; }} 
        public string Label { get { return m_Label; }} 
        public string Description { get { return m_Description; }} 
        public Character RelatedCharacter { get { return m_RelatedCharacter; }}
        
        public string PlayerPrefKey 
        {
            get { return $"TROPHY-{name.ToUpper()}"; }
        }

        public bool Unlocked
		{
			get { return PlayerPrefs.HasKey(this.PlayerPrefKey); }
		}

		public bool Unlock()
		{
			if (!this.Unlocked) {
				PlayerPrefs.SetString(this.PlayerPrefKey, DateTime.Now.ToString("U")  );
				PlayerPrefs.Save();
				return true;
			}
			return false;
		}

        public string GetUnlockDate()
		{
			return UnityEngine.PlayerPrefs.GetString(this.PlayerPrefKey, "");
		}

        public static void WipeUnlockData() {
            Trophy[] trophies = Resources.LoadAll<Trophy>(ResourcePaths.SO_TROPHIES);
            
            foreach (var t in trophies) 
                PlayerPrefs.DeleteKey(t.PlayerPrefKey);


            PlayerPrefs.Save();
        }

    }
}