using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheArchitect.Core.Controllers;

namespace TheArchitect.Core.Controllers
{

    public class RigidBody2DSpeedMatcher : StateMachineBehaviour
    {
        private Rigidbody2D rigidbody;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rigidbody = animator.GetComponent<Rigidbody2D>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (rigidbody != null) 
            {
                animator.speed = Mathf.Abs(rigidbody.velocity.x);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
           animator.speed = 1;
        }

    }
}
