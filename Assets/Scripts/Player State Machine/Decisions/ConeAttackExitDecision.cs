using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/ConeAttackExitDecision")]
public class ConeAttackExitDecision : Decision
{
    public override bool Decide(Player player)
    {
        return player.comeBackFromConeAttack;
    }
}