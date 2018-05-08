using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/CanonTrapEnterAction")]
public class CanonTrapEnterAction : StateAction
{
    public override void Act(Player player)
    {
        GameManager.instance.SetCrosshairActivate(false);
        player.bulletSpawnPoint.SetParent(player.currentTrap.transform);
    }
}
