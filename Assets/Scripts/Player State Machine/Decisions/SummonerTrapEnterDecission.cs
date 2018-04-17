using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/SummonerTrapEnterCheck")]
public class SummonerTrapEnterDecission : Decision
{
    public override bool Decide(Player player)
    {
        return player.nearbyTrap && player.timeSinceLastTrapUse > player.trapUseCooldown && InputManager.instance.GetXButtonDown() && player.nearbyTrap.CanUse() && player.nearbyTrap.trapType == Trap.TrapTypes.SUMMONER;
    }
}
