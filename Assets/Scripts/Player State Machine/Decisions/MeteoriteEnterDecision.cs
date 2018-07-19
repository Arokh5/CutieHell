using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/MeteoriteEnterDecision")]
public class MeteoriteEnterDecision : Decision
{
    public override bool Decide(Player player)
    {
        return InputManager.instance.GetTriangleButtonDown() && player.timeSinceLastMeteoriteAttack > player.meteoriteCooldown;
    }
}
