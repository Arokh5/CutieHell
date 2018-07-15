﻿using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/GroundedExit")]
public class GroundedExit : StateAction
{
    public override void Act(Player player)
    {
        player.SetCollidersActiveState(true);
        UIManager.instance.SetPlayerHealthButtonMashVisibility(false);
    }
}
