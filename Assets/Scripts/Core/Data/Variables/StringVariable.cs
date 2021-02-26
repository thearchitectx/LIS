using UnityEngine;

namespace TheArchitect.Core.Data.Variables
{
    [CreateAssetMenu(fileName = "New String", menuName = "Data/String Variable")]
    public class StringVariable : ScriptableObject
    {
        [SerializeField] private string m_Value;
        public string Value { get { return m_Value; } set { this.m_Value = value; }  }

        public StringVariable() { }
        
        public StringVariable(string value) {
            this.m_Value = Value;
        }
    }
}