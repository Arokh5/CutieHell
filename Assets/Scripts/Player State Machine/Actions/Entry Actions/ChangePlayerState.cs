using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ChangePlayerState")]
public class ChangePlayerState : StateAction {

    public Player.PlayerStates targetPlayerState;

    public override void Act(Player player)
    {
        player.state = targetPlayerState;
    }
}
