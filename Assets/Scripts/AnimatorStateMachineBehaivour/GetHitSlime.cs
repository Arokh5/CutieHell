using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetHitSlime : StateMachineBehaviour {

    private NavMeshAgent navMeshAgent;
    private AIEnemy aiEnemy;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckNavMeshAgent(animator);
        animator.SetBool("GetHit", false);
        navMeshAgent.speed = 0.0f;
	}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckNavMeshAgent(animator);
        CheckAIEnemy(animator);
        navMeshAgent.speed = aiEnemy.initialSpeed;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void CheckNavMeshAgent(Animator animator)
    {
        if (!navMeshAgent)
            navMeshAgent = animator.GetComponent<NavMeshAgent>();
    }

    private void CheckAIEnemy(Animator animator)
    {
        if (!aiEnemy)
            aiEnemy = animator.GetComponent<AIEnemy>();
    }
}
