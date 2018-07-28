using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/MeteoriteEnterDecision")]
public class MeteoriteEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        if (InputManager.instance.GetTriangleButtonDown())
        {
            if (player.meteoriteAttackCooldown.timeSinceLastAction > player.meteoriteAttackCooldown.cooldownTime)
                return true;
            else
                player.meteoriteAttackCooldown.cooldownUI.Flash();
        }
        return false;
    }
}
