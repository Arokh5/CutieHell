using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ConeAttackExitAction")]
public class ConeAttackExitAction : StateAction
{
    public override void Act(Player player)
    {
        player.canMove = true;
    }
}