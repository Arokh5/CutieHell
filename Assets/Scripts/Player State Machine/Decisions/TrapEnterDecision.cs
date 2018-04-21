using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/TrapEnterDecision")]
public class TrapEnterDecision : Decision
{
    public Trap.TrapTypes trapType;

    public override bool Decide(Player player)
    {
        return player.nearbyTrap
            && player.timeSinceLastTrapUse > player.trapUseCooldown
            && InputManager.instance.GetXButtonDown()
            && player.nearbyTrap.CanUse()
            && player.evilLevel >= player.nearbyTrap.usageCost
            && player.nearbyTrap.trapType == trapType;
    }
}
