using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ConeAttackEnterAction")]
public class ConeAttackEnterAction : StateAction
{

    public override void Act(Player player)
    {
        player.AddEvilPoints(-player.coneAttackEvilCost);
        player.comeBackFromConeAttack = false;
        player.timeSinceLastConeAttack = 0.0f;
        player.animator.SetTrigger("ConeAttack");
    }
}