using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/GroundedEnter")]
public class GroundedEnter : StateAction
{
    public override void Act(Player player)
    {
        player.elapsedRecoveryTime = 0.0f;
        UIManager.instance.SetPlayerHealthButtonMashVisibility(true);
    }
}
