using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongFollowUpEnterAction")]
public class StrongFollowUpEnter : StateAction
{
    public override void Act(Player player)
    {
        player.canMove = false;
        player.comeBackFromStrongAttack = false;
        player.strongAttackTimer = 0.0f;
        player.teleported = false;
        player.teleportState = Player.TeleportStates.IN;
        player.strongAttackCollider.Activate(false);
    }
}
