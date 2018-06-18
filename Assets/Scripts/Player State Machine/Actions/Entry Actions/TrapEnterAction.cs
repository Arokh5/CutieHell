using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/TrapEnterAction")]
public class TrapEnterAction : StateAction
{
    public override void Act(Player player)
    {
        player.currentTrap = player.nearbyTrap;
        player.currentTrap.Activate(player);
        player.currentTrap.GetCurrentTrapIndicator().gameObject.SetActive(true);
        player.SetIsAutoRecoveringEvil(false);
        player.SetRenderersVisibility(false);
    }
}
