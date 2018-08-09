using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/AttackChainDecision")]
public class AttackChainDecision : Decision
{
    public override bool Decide(Player player)
    {
        foreach (ControllerButton button in AttackInfosManager.instance.allButtons)
        {
            if (InputManager.instance.GetButtonDown(button))
            {
                AttackInfo info = AttackInfosManager.instance.GetAttackInfo(button);
                return AttackChainsManager.instance.ReportFollowUpAttempt(info.type);
            }
        }
        return false;
    }
}
