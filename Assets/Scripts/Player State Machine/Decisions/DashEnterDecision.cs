using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/DashEnterDecision")]
public class DashEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        if (InputManager.instance.GetL2ButtonDown())
        {
            if (player.dashCooldown.timeSinceLastAction >= player.dashCooldown.cooldownTime)
                return true;
            else
                player.dashCooldown.cooldownUI.Flash();
        }
        return false;
    }
}
