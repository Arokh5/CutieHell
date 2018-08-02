using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/DashExitDecision")]
public class DashExitDecision : Decision
{
    public override bool Decide(Player player)
    {
        return player.dashElapsedTime > player.dashDuration
            || player.knockbackActive;
    }
}
