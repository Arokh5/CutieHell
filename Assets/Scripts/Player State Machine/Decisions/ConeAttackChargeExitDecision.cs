using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/ConeAttackChargeExitDecision")]
public class ConeAttackChargeExitDecision : Decision
{

    public override bool Decide(Player player)
    {
        return player.slashButtonReleased;
    }
}
