using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/BasicAttackExit")]
public class BasicAttackExit : StateAction
{
    public override void Act(Player player)
    {
        UIManager.instance.HideRepairText();
        UIManager.instance.HideUseText();
        if (player.currentBasicAttackTarget)
        {
            player.currentBasicAttackTarget.MarkAsTarget(false);
            player.currentBasicAttackTarget = null;
        }
        player.footSteps.SetActive(false);
    }
}
