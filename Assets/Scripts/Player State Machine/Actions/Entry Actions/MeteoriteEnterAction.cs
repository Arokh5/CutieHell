using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/MeteoriteEnterAction")]
public class MeteoriteEnterAction : StateAction
{
    public override void Act(Player player)
    {
        player.AddEvilPoints(-player.meteoriteAttackEvilCost);
        player.meteoritesShot = false;
    }
}