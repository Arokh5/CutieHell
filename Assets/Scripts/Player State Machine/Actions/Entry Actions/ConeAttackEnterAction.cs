using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ConeAttackEnterAction")]
public class ConeAttackEnterAction : StateAction
{
    public override void Act(Player player)
    {
        player.comeBackFromConeAttack = false;
        player.coneAttackCooldown.timeSinceLastAction = 0.0f;
        player.mainCameraController.timeSinceLastAction = 0.0f;
        player.mainCameraController.fastAction = true;
        player.animator.SetTrigger("ConeAttack");
    }
}