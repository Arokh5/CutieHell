using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/GroundedEnterDecision")]
public class GroundedEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        return player.IsDead();
    }
}
