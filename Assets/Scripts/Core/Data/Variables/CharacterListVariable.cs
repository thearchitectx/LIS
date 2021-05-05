using UnityEngine;

namespace TheArchitect.Core.Data.Variables
{
    [CreateAssetMenu(fileName = "New Character List Variable", menuName = "Data/Character List Variable")]
    public class CharacterListVariable : ScriptableObject
    {
        [SerializeField] private Character[] m_Value;
        public Character[] Value { get { return m_Value; } set { this.m_Value = value; }  }

        public CharacterListVariable() { }
        
        public CharacterListVariable(string value) {
            this.m_Value = Value;
        }
    }
}