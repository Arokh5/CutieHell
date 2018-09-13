using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlaceMine")]
public class UpdateMineTimer : StateAction
{
    public override void Act(Player player)
    {
        UpdateTimer(player);
    }

    private void UpdateTimer(Player player)
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
