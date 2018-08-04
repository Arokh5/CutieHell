using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/GroundedRecover")]
public class GroundedRecover : StateAction
{
    public override void Act(Player player)
    {
        bool recoveryOver = Recovery(player);
        if (recoveryOver)
        {
            bool delayOver = Delay(player);
            if (delayOver)
                player.isGrounded = false;
        }
    }

    private bool Recovery(Player player)
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
            UIManager.instance.SetPlayerHealthButtonMashVisibility(false);
        }

        return player.elapsedRecoveryTime >= player.recoveryDuration;
    }

    private bool Delay(Player player)
    {
        player.elapsedDelayTime += Time.deltaTime;
        return player.elapsedDelayTime >= player.postRecoveryDelay;
    }
}
