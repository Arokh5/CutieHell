using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBearAnimatorBehaivour : StateMachineBehaviour {

    private AIEnemy aiEnemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckAIEnemy(animator);
        animator.SetBool("Dead", true);
        aiEnemy.SetIsTargetable(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 1.25f)
        {
            CheckAIEnemy(animator);
            aiEnemy.Die();
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void CheckAIEnemy(Animator animator)
    {
        if (!aiEnemy)
            aiEnemy = animator.GetComponent<AIEnemy>();
    }
}
