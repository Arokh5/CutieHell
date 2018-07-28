using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/StrongAttackEnterDecision")]
public class StrongAttackDecision : Decision
{
    public override bool Decide(Player player)
    {
        if (InputManager.instance.GetOButtonDown())
        {
            if (player.strongAttackCooldown.timeSinceLastAction >= player.strongAttackCooldown.cooldownTime)
                return true;
            else
                player.strongAttackCooldown.cooldownUI.Flash();
        }
        return false;
    }
}
