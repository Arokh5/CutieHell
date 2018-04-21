using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/UseTrapMessage")]
public class UseTrapMessage : StateAction
{
    public override void Act(Player player)
    {
        bool showMessage = player.nearbyTrap && player.nearbyTrap.CanUse() && player.nearbyTrap.usageCost <= player.evilLevel;
        
        if (showMessage)
            UIManager.instance.ShowUseText();
        else
            UIManager.instance.HideUseText();
    }
}
