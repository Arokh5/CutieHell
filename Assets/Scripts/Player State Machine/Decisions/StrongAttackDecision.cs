using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/StrongAttackEnterDecision")]
public class StrongAttackDecision : Decision
{
    public override bool Decide(Player player)
    {
        return player.strongAttackStateCooldown < player.timeSinceLastStrongAttack
           && player.GetEvilLevel() > player.strongAttackEvilCost
           && InputManager.instance.GetOButtonDown();
    }
}
