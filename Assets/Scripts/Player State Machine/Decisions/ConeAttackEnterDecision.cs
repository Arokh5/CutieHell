using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/ConeAttackEnterDecision")]
public class ConeAttackEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        if (InputManager.instance.GetSquareButtonDown())
        {
            if (player.coneAttackCooldown.timeSinceLastAction >= player.coneAttackCooldown.cooldownTime)
                return true;
            else
                player.coneAttackCooldown.cooldownUI.Flash();
        }
        return false;
    }
}
