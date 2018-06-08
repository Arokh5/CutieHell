using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttackExit")]
public class StrongAttackExit : StateAction
{
    public override void Act(Player player)
    {
        player.timeSinceLastStrongAttack = 0.0f;
        player.SetRenderersVisibility(true);
        player.cameraState = Player.CameraState.MOVE;
        player.canMove = true;
        /* Untarget all enemies */
        foreach (AIEnemy aiEnemy in player.currentStrongAttackTargets)
        {
            aiEnemy.MarkAsTarget(false);
        }
        player.currentStrongAttackTargets.Clear();
    }
}
