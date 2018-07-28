using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/ConeAttackEnterDecision")]
public class ConeAttackEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        return player.coneAttackCooldown.timeSinceLastAction > player.coneAttackCooldown.cooldownTime
            && InputManager.instance.GetSquareButtonDown();
    }
}
