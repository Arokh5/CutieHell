using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/TrapExitAction")]
public class TrapExitAction : StateAction {

    public override void Act(Player player)
    {
        player.shouldExitTrap = false;
        player.bulletSpawnPoint.SetParent(player.transform);
        player.bulletSpawnPoint.localPosition = player.initialBulletSpawnPointPos;
        player.bulletSpawnPoint.rotation = Quaternion.identity;
        player.currentTrap.Deactivate();
        player.SetRenderersVisibility(true);

        Vector3 eulerRotation = player.currentTrap.rotatingHead.rotation.eulerAngles;
        eulerRotation.x = 0;
        player.currentTrap.rotatingHead.rotation = Quaternion.Euler(eulerRotation);

        Vector3 nextPos = player.currentTrap.transform.forward * 3f;
        player.transform.position = new Vector3(player.currentTrap.transform.position.x - nextPos.x, player.transform.position.y, player.currentTrap.transform.position.z - nextPos.z);
        player.currentTrap = null;
        player.timeSinceLastTrapUse = 0f;
    }
}
