using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttackAnimator : StateMachineBehaviour {

    public float attackSpawnTime;
    private bool flag;
    private Player player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckPlayer(animator);
        player.canMove = false;
        flag = false;
        BulletTime.instance.DoSlowmotion(0.001f,0.75f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= attackSpawnTime && !flag)
        {
            flag = true;
            CheckPlayer(animator);
            ParticlesManager.instance.LaunchParticleSystem(player.coneAttackVFX, player.transform.position, player.transform.rotation);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckPlayer(animator);
        player.comeBackFromConeAttack = true;
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
        if (player == null)
            player = animator.GetComponent<Player>();
    }
}
