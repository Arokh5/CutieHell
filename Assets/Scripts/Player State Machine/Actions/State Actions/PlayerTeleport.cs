using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlayerTeleport")]
public class PlayerTeleport : StateAction
{
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
                    if (player.currentTelepotTarget != null)
                    {
                        player.transform.position = player.currentTelepotTarget.transform.position;
                        player.transform.rotation = player.currentTelepotTarget.transform.rotation;
                        player.SetZoneController(player.currentTelepotTarget.zoneController);
                        Camera.main.GetComponent<CameraController>().SetCameraXAngle(player.currentTelepotTarget.transform.rotation.eulerAngles.y);
                        player.currentTelepotTarget = null;
                    }
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