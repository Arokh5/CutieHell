using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/TeleportExitAction")]
public class TeleportExitAction : StateAction
{
    public GameObject teleportOut;
    public override void Act(Player player)
    {
        player.SetRenderersVisibility(true);
        player.cameraState = Player.CameraState.MOVE;
        player.timeSinceLastTeleport = 0.0f;
        Destroy(Instantiate(teleportOut, player.transform.position, teleportOut.transform.rotation), 1.5f);
    }
}
