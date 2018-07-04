using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/GroundedRecover")]
public class GroundedRecover : StateAction
{
    public override void Act(Player player)
    {
        player.elapsedRecoveryTime += Time.deltaTime;
        if (InputManager.instance.GetXButtonDown())
        {
            player.elapsedRecoveryTime += player.timeSavedPerClick;
        }


        if (player.elapsedRecoveryTime < player.recoveryDuration)
        {
            player.SetCurrentHealth(player.elapsedRecoveryTime / player.recoveryDuration);
        }
        else
        {
            player.SetCurrentHealth(1.0f);
            player.isGrounded = false;
        }
    }
}
