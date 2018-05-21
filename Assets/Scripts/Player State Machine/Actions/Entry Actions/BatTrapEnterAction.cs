using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/BatTrapEnterAction")]
public class BatTrapEnterAction : StateAction
{
    public override void Act(Player player)
    {
        player.AddEvilPoints(-player.currentTrap.usageCost);

        player.bulletSpawnPoint.SetParent(player.currentTrap.rotatingHead);
        player.bulletSpawnPoint.localPosition = new Vector3(0f, 0.3f, 0.7f);
    }
}
