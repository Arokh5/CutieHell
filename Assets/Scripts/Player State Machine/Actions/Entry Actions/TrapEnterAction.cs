using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/TrapEnterAction")]
public class TrapEnterAction : StateAction {

    public override void Act(Player player)
    {
        UIManager.instance.HideRepairTrapText();
        player.currentTrap = player.nearbyTrap;
        player.bulletSpawnPoint.SetParent(player.nearbyTrap.transform);
        player.bulletSpawnPoint.localPosition = new Vector3(0f, 0.3f, 0.7f);
        player.currentTrap.Activate(player);
        player.meshRenderer.enabled = false;
    }
}
