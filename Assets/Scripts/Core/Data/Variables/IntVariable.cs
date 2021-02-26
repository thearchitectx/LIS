using UnityEngine;

namespace TheArchitect.Core.Data.Variables
{
    [CreateAssetMenu(fileName = "New Float", menuName = "Data/Int Variable")]
    public class IntVariable : ScriptableObject
    {
        [SerializeField] private int m_Value;
        public int Value { get { return m_Value; } }

        public IntVariable() { }
        
        public IntVariable(float value) {
            this.m_Value = Value;
        }
    }
}