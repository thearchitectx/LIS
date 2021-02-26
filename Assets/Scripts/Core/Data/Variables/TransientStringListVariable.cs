using System.Collections.Generic;
using UnityEngine;

namespace TheArchitect.Core.Data.Variables
{
    [CreateAssetMenu(fileName = "New String List", menuName = "Data/Transient/String List Variable")]
    public class TransientStringListVariable : ScriptableObject
    {
        [System.NonSerialized] private List<string> m_Value;
        public List<string> Value { get { return m_Value == null ? m_Value = new List<string>() : m_Value; } }
    }
}