using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/GroundedRecover")]
public class GroundedRecover : StateAction
{
    public override void Act(Player player)
    {
        bool recoveryOver = Recovery(player);
        if (recoveryOver)
        {
            player.isGrounded = false;
        }
    }

    private bool Recovery(Player player)
    {
        player.elapsedRecoveryTime += Time.deltaTime;
        if (player.elapsedRecoveryTime < player.recoveryDuration)
        {
            player.SetCurrentHealth(player.elapsedRecoveryTime / player.recoveryDuration);
        }
        else
        {
            player.SetCurrentHealth(1.0f);
        }

        return player.elapsedRecoveryTime >= player.recoveryDuration;
    }
}
