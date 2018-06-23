using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/MoveStateEnter")]
public class MoveStateEnter : StateAction
{
    public override void Act(Player player)
    {
        player.loopAudioSource.clip = player.footstepsClip;
    }
}
