using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/TeleportBaseEnterAction")]
public class TeleportBaseEnterAction : StateAction
{
    public ParticleSystem teleportIn;

    public override void Act(Player player)
    {
        player.SetRenderersVisibility(false);
        player.teleported = false;
        player.timeSinceLastTeleport = 0.0f;
        player.cameraState = Player.CameraState.ZOOMOUT;
        player.teleportState = Player.TeleportStates.OUT;
        ParticlesManager.instance.LaunchParticleSystem(teleportIn, player.transform.position, teleportIn.transform.rotation);
    }
}
