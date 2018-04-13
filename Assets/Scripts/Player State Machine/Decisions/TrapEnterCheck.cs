using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/TrapEnterCheck")]
public class TrapEnterCheck : Decision
{
    public override bool Decide(Player player)
    {
        return player.nearbyTrap && InputManager.instance.GetXButtonDown() && player.nearbyTrap.CanUse();
    }
}
