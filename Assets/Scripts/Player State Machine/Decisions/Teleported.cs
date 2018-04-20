using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/Teleported")]
public class Teleported : Decision
{
    public override bool Decide(Player player)
    {
        return player.teleported;
    }
}

