using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongFollowUpEnterAction")]
public class StrongFollowUpEnter : StateAction
{
    public override void Act(Player player)
    {
        player.canMove = false;
        player.animator.SetTrigger("StrongAttackFollow");
        player.comeBackFromStrongAttack = false;
        player.strongAttackTimer = 0.0f;
        player.teleported = false;
        player.cameraState = Player.CameraState.CONEATTACK;
        player.teleportState = Player.JumpStates.LAND;
        player.strongAttackCollider.Activate(false);
    }
}
