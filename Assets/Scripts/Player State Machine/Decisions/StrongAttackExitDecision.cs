using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/StrongAttackExitDecision")]
public class StrongAttackExitDecision : Decision
{
    public override bool Decide(Player player)
    {
        return player.comeBackFromStrongAttack;
    }
}
