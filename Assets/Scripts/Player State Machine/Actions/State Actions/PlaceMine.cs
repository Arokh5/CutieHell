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
            if (player.mineAttackCooldown.timeSinceLastAction >= player.mineAttackCooldown.cooldownTime
                //&& player.availableMinesNumber > 0
                )
            {
                tutorialEventLauncher.LaunchEvent();
                player.mineAttackCooldown.timeSinceLastAction = 0.0f;
                player.InstantiateMine();
            }
            else
                player.mineAttackCooldown.cooldownUI.Flash();
        }

        //UpdateMineTimer(player);
    }

    private void UpdateMineTimer(Player player)
    {
        if (player.availableMinesNumber < player.maxMinesNumber)
        {
            player.timeSinceLastMine += Time.deltaTime;
            if (player.timeSinceLastMine >= player.timeToGetAnotherMine)
            {
                player.availableMinesNumber++;
                player.timeSinceLastMine = 0.0f;
            }
        }
    }
}
