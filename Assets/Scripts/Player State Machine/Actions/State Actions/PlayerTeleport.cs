using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlayerTeleport")]
public class PlayerTeleport : StateAction
{
    public Transform destination;
    public float timeToGoOut, timeToTravel, timeToGoIn;

    public override void Act(Player player)
    {
        player.timeSinceLastTeleport += Time.deltaTime;

        switch (player.teleportState)
        {
            case Player.TeleportStates.OUT:
                Debug.Log("WAAAA");
                if (player.timeSinceLastTeleport >= timeToGoOut)
                {
                    player.timeSinceLastTeleport = 0.0f;
                    player.cameraState = Player.CameraState.ZOOMOUT;
                    player.teleportState = Player.TeleportStates.TRAVEL;

                }
                break;
            case Player.TeleportStates.TRAVEL:
                Debug.Log("WEEEE");
                if (player.timeSinceLastTeleport >= timeToTravel)
                {
                    player.timeSinceLastTeleport = 0.0f;
                    player.cameraState = Player.CameraState.TRANSITION;
                    player.teleportState = Player.TeleportStates.IN;
                }
                break;
            case Player.TeleportStates.IN:
                Debug.Log("WIIII");
                if (player.timeSinceLastTeleport >= timeToGoIn)
                {
                    player.timeSinceLastTeleport = 0.0f;
                    player.cameraState = Player.CameraState.ZOOMIN;
                    player.teleported = true;
                }

                break;
            default:
                break;
        }
    }
}