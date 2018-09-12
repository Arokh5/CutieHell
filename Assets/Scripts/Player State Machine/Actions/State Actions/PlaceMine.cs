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

        UpdateMineTimer(player);
    }

    private void UpdateMineTimer(Player player)
    {
        if (player.GetAvailableMines() < player.maxCurrentMinesNumber)
        {
            player.timeSinceLastMine += Time.deltaTime;
            if (player.timeSinceLastMine >= player.timeToGetAnotherMine)
            {
                player.GetNewMine();
                player.timeSinceLastMine = 0.0f;
            }
            player.SetPercentageToNextMine(player.timeSinceLastMine / player.timeToGetAnotherMine);
        }
    }
}
