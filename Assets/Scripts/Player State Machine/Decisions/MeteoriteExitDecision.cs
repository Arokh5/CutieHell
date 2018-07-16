using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/MeteoriteExitDecision")]
public class MeteoriteExitDecision : Decision
{
    public override bool Decide(Player player)
    {
        return !player.IsDead();
    }
}
