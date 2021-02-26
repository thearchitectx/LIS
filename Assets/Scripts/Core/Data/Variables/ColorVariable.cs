using UnityEngine;

namespace TheArchitect.Core.Data.Variables
{
    [CreateAssetMenu(fileName = "New Float", menuName = "Data/Color Variable")]
    public class ColorVariable : ScriptableObject
    {
        [SerializeField] private Color m_Value;
        public Color Value { get { return m_Value; } }

        public ColorVariable() { }
        
        public ColorVariable(Color value)
        {
            this.m_Value = Value;
        }
    }
}