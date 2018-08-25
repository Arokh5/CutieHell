using UnityEngine;

[CreateAssetMenu(menuName = "Player Tutorial State Machine/Actions/StopMoveAnimation")]
public class StopMoveAnimation : StateAction
{
    public override void Act(Player player)
    {
        player.animator.SetBool("Move", false);
    }
}
