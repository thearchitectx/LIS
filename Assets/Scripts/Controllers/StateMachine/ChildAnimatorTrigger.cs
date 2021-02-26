using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAnimatorTrigger : StateMachineBehaviour
{
    [SerializeField] private string m_childAnimatorPath;
    [SerializeField] private string m_Trigger;
    [SerializeField] private string m_Int;
    [SerializeField] private int m_IntValue;
    [SerializeField] private string m_Bool;
    [SerializeField] private bool m_BoolValue;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var childTransform = animator.transform.Find(m_childAnimatorPath);
        var childAnimator = childTransform.GetComponent<Animator>();
        if (m_Trigger != "")
        {
            childAnimator.SetTrigger(m_Trigger);
        }
        if (m_Int != "")
        {
            childAnimator.SetInteger(m_Int, m_IntValue);
        }
        if (m_Bool != "")
        {
            childAnimator.SetBool(m_Bool, m_BoolValue);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
