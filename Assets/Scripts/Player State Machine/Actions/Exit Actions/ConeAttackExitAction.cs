using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ConeAttackExitAction")]
public class ConeAttackExitAction : StateAction
{
    public override void Act(Player player)
    {
        Achievements.instance.DestroyAchievementInstantiation(AchievementType.CONSECUTIVEHITTING, GameManager.instance.GetPlayer1().GetConeAttackLinkedAchievementID());
        player.canMove = true;
        player.mainCameraController.y = 5.0f;
        //player.cameraState = Player.CameraState.MOVE;
    }
}