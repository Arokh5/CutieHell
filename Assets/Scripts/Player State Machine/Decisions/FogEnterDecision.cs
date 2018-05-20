using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/FogEnterDecision")]
public class FogEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        return Time.time - player.fogStateCooldown > player.fogStateLastTime
           && player.GetEvilLevel() > 5
           && InputManager.instance.GetSquareButtonDown();
    }
}
