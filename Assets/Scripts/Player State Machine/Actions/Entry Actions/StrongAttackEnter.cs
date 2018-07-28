using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttackEnterAction")]
public class StrongAttackEnter : StateAction
{
    public ParticleSystem teleportIn;

    public override void Act(Player player)
    {
        player.AddEvilPoints(0);
        player.canMove = false;
        player.comeBackFromStrongAttack = false;
        player.strongAttackTimer = 0.0f;
        player.SetRenderersVisibility(false);
        player.teleported = false;
        player.timeSinceLastTeleport = 0.0f;
        player.initialPositionOnStrongAttack = player.transform;
        player.cameraState = Player.CameraState.STRONG_ATTACK;
        player.teleportState = Player.TeleportStates.OUT;
        player.SetIsAutoRecoveringEvil(false);
        ParticlesManager.instance.LaunchParticleSystem(teleportIn, player.transform.position, teleportIn.transform.rotation);
    }
}
