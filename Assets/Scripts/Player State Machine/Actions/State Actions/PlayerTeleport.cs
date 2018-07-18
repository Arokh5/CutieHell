using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlayerTeleport")]
public class PlayerTeleport : StateAction
{
    public int teleportTargetIndex;
    public float timeToGoOut, timeToTravel, timeToGoIn;
    public ParticleSystem teleportVFX;

    public override void Act(Player player)
    {
        player.timeSinceLastTeleport += Time.deltaTime;

        switch (player.teleportState)
        {
            case Player.TeleportStates.OUT:
                if (player.timeSinceLastTeleport >= timeToGoOut)
                {
                    player.timeSinceLastTeleport = 0.0f;
                    player.cameraState = Player.CameraState.TRANSITION;
                    player.teleportState = Player.TeleportStates.TRAVEL;
                    if (teleportTargetIndex >= 0 && teleportTargetIndex < player.teleportTargets.Length)
                        player.transform.position = player.teleportTargets[teleportTargetIndex].position;

                }
                break;
            case Player.TeleportStates.TRAVEL:
                if (player.timeSinceLastTeleport >= timeToTravel)
                {
                    player.timeSinceLastTeleport = 0.0f;
                    player.cameraState = Player.CameraState.ZOOMIN;
                    player.teleportState = Player.TeleportStates.IN;
                    ParticlesManager.instance.LaunchParticleSystem(teleportVFX, player.transform.position, teleportVFX.transform.rotation);
                }
                break;
            case Player.TeleportStates.IN:
                if (player.timeSinceLastTeleport >= timeToGoIn)
                {
                    player.timeSinceLastTeleport = 0.0f;
                    player.teleported = true;
                }

                break;
            default:
                break;
        }
    }
}