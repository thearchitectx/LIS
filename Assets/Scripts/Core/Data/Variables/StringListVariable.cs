using System.Collections.Generic;
using UnityEngine;

namespace TheArchitect.Core.Data.Variables
{
    [CreateAssetMenu(fileName = "New String List", menuName = "Data/String List Variable")]
    public class StringListVariable : ScriptableObject
    {
        [SerializeField] private List<string> m_Value;
        public List<string> Value { get { return m_Value; } }

        public StringListVariable() { }
        
        public StringListVariable(List<string> value) {
            this.m_Value = Value;
        }
    }
}