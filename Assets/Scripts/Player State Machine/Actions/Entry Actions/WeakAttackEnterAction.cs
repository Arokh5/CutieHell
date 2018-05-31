using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/WeakAttackEnterAction")]
public class WeakAttackEnterAction : StateAction
{

    public override void Act(Player player)
    {
        if (player.currentlyUsingTrap)
            player.currentlyUsingTrap = false;
    }
}
