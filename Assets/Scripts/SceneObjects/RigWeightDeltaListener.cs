using UnityEngine;
using UnityEngine.Animations.Rigging;

// Changes outcome after certain rig weight delta
namespace TheArchitect.SceneObjects
{
    public class RigWeightDeltaListener : SceneObject
    {
        public Rig Rig;
        public float DeltaMovement = 1;

        private float m_LastWeight;
        private float m_TotalDelta;

        void Update()
        {
            if (Rig == null) 
                return;
                
            this.m_TotalDelta += Mathf.Abs(Rig.weight - this.m_LastWeight);
            this.m_LastWeight = Rig.weight;

            this.Outcome = (this.m_TotalDelta >= DeltaMovement)
                ? "DELTA_ACHIEVED"
                : null;
        }
    }
}