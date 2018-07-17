using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/MeteoriteEnterAction")]
public class MeteoriteEnterAction : StateAction
{
    public override void Act(Player player)
    {
        player.AddEvilPoints(-player.meteoriteAttackEvilCost);
        player.comeBackFromMeteoriteAttack = false;
        player.transform.position = new Vector3(player.transform.position.x, 55.0f, player.transform.position.z);
        player.transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}