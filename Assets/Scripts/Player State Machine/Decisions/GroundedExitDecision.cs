using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/GroundedExitDecision")]
public class GroundedExitDecision : Decision
{
    public override bool Decide(Player player)
    {
        return !player.IsDead();
    }
}
