using UnityEngine;
using TheArchitect.Core.Constants;

namespace TheArchitect.Core.Data
{

    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private string m_Label;
        [SerializeField] private int m_Price;
        [SerializeField] [TextArea] private string m_Description;

        public string Label { get { return this.m_Label; } }
        public string LabelUpper { get { return this.m_Label.ToUpper(); } }
        public string Description { get { return this.m_Description; } }
        public int Price { get { return this.m_Price; } }

        public Sprite Icon {
            get { return Resources.Load<Sprite>(IconPath); }
        }

        public string IconPath {
            get { return $"{ResourcePaths.SO_ITEMS}/{this.name}"; }
        }
    }

}
