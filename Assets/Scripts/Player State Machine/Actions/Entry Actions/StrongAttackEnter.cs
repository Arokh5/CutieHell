using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttackEnterAction")]
public class StrongAttackEnter : StateAction
{
    public ParticleSystem teleportIn;

    public override void Act(Player player)
    {
        player.canChargeStrongAttack = false;
        player.isChargingStrongAttack = false;
        player.canMove = false;
        player.comeBackFromStrongAttack = false;
        player.strongAttackTimer = 0.0f;
        player.SetRenderersVisibility(false);
        player.strongAttackMotionLimiter.SetActive(true);
        player.teleported = false;
        player.cameraState = Player.CameraState.STRONG_ATTACK;
        player.teleportState = Player.JumpStates.JUMP;
        ParticlesManager.instance.LaunchParticleSystem(teleportIn, player.transform.position, teleportIn.transform.rotation);
    }
}
