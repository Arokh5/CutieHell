using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttackEnterAction")]
public class StrongAttackEnter : StateAction
{
    public ParticleSystem teleportIn;
    public int evilCost;

    public override void Act(Player player)
    {
        player.AddEvilPoints(-evilCost);
        player.canMove = false;
        player.comeBackFromStrongAttack = false;
        player.timeSinceLastStrongAttack = 0.0f;
        player.SetRenderersVisibility(false);
        player.teleported = false;
        player.timeSinceLastTeleport = 0.0f;
        player.initialPositionOnStrongAttack = player.transform;
        player.cameraState = Player.CameraState.FOG;
        player.teleportState = Player.TeleportStates.OUT;
        ParticlesManager.instance.LaunchParticleSystem(teleportIn, player.transform.position, teleportIn.transform.rotation);
    }
}
