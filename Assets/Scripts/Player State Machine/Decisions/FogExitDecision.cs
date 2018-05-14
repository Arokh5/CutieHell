using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/FogExitDecision")]
public class FogExitDecision : Decision
{
    public override bool Decide(Player player)
    {
        return Time.time - player.fogStateCooldown > player.fogStateLastTime && InputManager.instance.GetSquareButtonDown()
           || player.evilLevel <= 0.0f;
    }
}
