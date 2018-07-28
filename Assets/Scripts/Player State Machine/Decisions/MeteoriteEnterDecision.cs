using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/MeteoriteEnterDecision")]
public class MeteoriteEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        return player.meteoriteAttackCooldown.timeSinceLastAction > player.meteoriteAttackCooldown.cooldownTime
            &&InputManager.instance.GetTriangleButtonDown();
    }
}
