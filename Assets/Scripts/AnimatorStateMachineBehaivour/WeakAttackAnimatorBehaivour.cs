using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAttackAnimatorBehaivour : StateMachineBehaviour {

    public float shotTriggerNormalizedTime;
    public ParticleSystem prefab;
    private bool shotDone = false;
    private Player player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckPlayer(animator);
        player.animatingAttack = true;
        shotDone = false;
        player.mainCameraController.timeSinceLastAction = 0.0f;
        player.mainCameraController.slowAction = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckPlayer(animator);
        if (stateInfo.normalizedTime >= shotTriggerNormalizedTime && !shotDone)
        {
            shotDone = true;
            player.InstantiateAttack(prefab, player.weakAttackTargetTransform, player.weakAttackTargetHitPoint);
            player.weakAttackTargetTransform = null;
            player.weakAttackTargetHitPoint = Vector3.zero;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckPlayer(animator);
        player.animatingAttack = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void CheckPlayer(Animator animator)
    {
        if (!player)
            player = animator.GetComponent<Player>();
    }
}
