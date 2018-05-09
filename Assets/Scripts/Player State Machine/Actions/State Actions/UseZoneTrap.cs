using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/UseZoneTrap")]
public class UseZoneTrap : StateAction
{
    public override void Act(Player player)
    {
        if (player.zoneTrap && Input.GetKeyDown(KeyCode.A))//InputManager.instance.GetOButtonDown())
        {
            if (player.zoneTrap.CanUse() && player.GetEvilLevel() >= player.zoneTrap.GetUsageCost())
            {
                player.AddEvilPoints(-player.zoneTrap.GetUsageCost());
                player.zoneTrap.UseZoneTrap();
            }
        }
    }
}
