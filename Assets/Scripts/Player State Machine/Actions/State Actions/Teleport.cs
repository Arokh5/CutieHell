using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/TeleportAction")]
public class Teleport : StateAction
{
    public override void Act(Player player)
    {
        if (InputManager.instance.GetL1ButtonDown())
            player.transform.position = player.statueTeleportPoint.position;

        if (InputManager.instance.GetR1ButtonDown())
            player.transform.position = player.centerTeleportPoint.position;
    }
}
