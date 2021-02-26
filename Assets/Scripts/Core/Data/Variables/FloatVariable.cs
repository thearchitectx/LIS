using UnityEngine;

namespace TheArchitect.Core.Data.Variables
{
    [CreateAssetMenu(fileName = "New Float", menuName = "Data/Float Variable")]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField] private float m_Value;
        public float Value { get { return m_Value; } }

        public FloatVariable() { }
        
        public FloatVariable(float value) 
        {
            this.m_Value = Value;
        }
    }
}