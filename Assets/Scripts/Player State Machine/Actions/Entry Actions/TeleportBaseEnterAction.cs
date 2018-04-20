using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/TeleportBaseEnterAction")]
public class TeleportBaseEnterAction : StateAction
{
    public GameObject teleportIn;

    public override void Act(Player player)
    {
        player.SetRenderersVisibility(false);
        player.timeSinceLastTeleport = 0.0f;
        player.teleportState = Player.TeleportStates.OUT;
        Destroy(Instantiate(teleportIn, player.transform.position, teleportIn.transform.rotation), 1.5f);
    }
}
