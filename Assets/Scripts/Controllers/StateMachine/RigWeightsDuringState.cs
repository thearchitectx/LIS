using TheArchitect.Core.Constants;
using UnityEngine;

[System.Serializable]
public struct RigWeightDuringStateData
{
   [SerializeField] public SkeletonRig Rig;
   [SerializeField] public float TargetWeight;
}

public class RigWeightsDuringState : StateMachineBehaviour
{
   
   [SerializeField] private RigWeightDuringStateData[] m_Data;
   private RigWeightDamper[] m_Rigs;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       this.m_Rigs = new RigWeightDamper[this.m_Data.Length];
       for (var i = 0; i < this.m_Data.Length; i++)
         this.m_Rigs[i] = animator.transform.Find(SkeletonPaths.GetPathTo(this.m_Data[i].Rig)).GetComponent<RigWeightDamper>();
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       for (var i = 0; i < this.m_Data.Length; i++)
         this.m_Rigs[i].SetWeightKeepRestore(this.m_Data[i].TargetWeight);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      for (var i = 0; i < this.m_Rigs.Length; i++)
         this.m_Rigs[i].RestoreWeight();
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
