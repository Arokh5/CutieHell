using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttackEnterAction")]
public class StrongAttackEnter : StateAction
{
    public override void Act(Player player)
    {
        player.strongAttackObject.SetActive(true);
    }
}
