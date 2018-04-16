using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttackExit")]
public class StrongAttackExit : StateAction
{
    public override void Act(Player player)
    {
        player.strongAttackObject.SetActive(false);
        player.strongAttackMeshCollider.enabled = false;

        /* Untarget all enemies */
        foreach (AIEnemy aiEnemy in player.currentStrongAttackTargets)
        {
            aiEnemy.MarkAsTarget(false);
        }
        player.currentStrongAttackTargets.Clear();
    }
}
