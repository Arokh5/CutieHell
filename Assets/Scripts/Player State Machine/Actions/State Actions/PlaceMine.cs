using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlaceMine")]
public class PlaceMine : StateAction
{
    [SerializeField]
    private TutorialEventLauncher tutorialEventLauncher;

    public override void Act(Player player)
    {
        if (InputManager.instance.GetXButtonDown())
        {
            if (player.GetAvailableMines() > 0)
            {
                tutorialEventLauncher.LaunchEvent();
                player.InstantiateMine();
            }
            else
                player.mineAttackCooldown.cooldownUI.Flash();
        }
    }
}
