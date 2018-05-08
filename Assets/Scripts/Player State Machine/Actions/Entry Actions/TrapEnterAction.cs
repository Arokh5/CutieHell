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
        player.SetRenderersVisibility(false);
        player.AddEvilPoints(-player.currentTrap.usageCost);

        player.bulletSpawnPoint.SetParent(player.currentTrap.rotatingHead);
        player.bulletSpawnPoint.localPosition = new Vector3(0f, 0.3f, 0.7f);
    }
}
