using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/MoveStateExit")]
public class MoveStateExit : StateAction
{
    public override void Act(Player player)
    {
        player.loopAudioSource.Stop();
        player.animator.SetBool("Move", false);
    }
}
