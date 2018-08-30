using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttackExit")]
public class StrongAttackExit : StateAction
{
    public override void Act(Player player)
    {
        player.strongAttackTimer = 0.0f;
        player.strongAttackCollider.Deactivate();
        player.cameraState = Player.CameraState.MOVE;
        player.canMove = true;

        foreach (AIEnemy aiEnemy in player.currentStrongAttackTargets)
        {
            aiEnemy.MarkAsTarget(false);
        }
        player.currentStrongAttackTargets.Clear();
    }
}
