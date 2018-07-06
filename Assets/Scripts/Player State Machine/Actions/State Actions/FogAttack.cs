using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/FogAttack")]
public class FogAttack : StateAction
{
    public float evilCostPerSecond;
    public float hitInterval;
    public float dps;
    [SerializeField]
    private float decreaseRate;

    public override void Act(Player player)
    {
        player.accumulatedFogEvilCost += evilCostPerSecond * Time.deltaTime;
        if (player.accumulatedFogEvilCost > decreaseRate)
        {
            player.accumulatedFogEvilCost -= decreaseRate;
            player.AddEvilPoints(-decreaseRate);
        }

        player.timeSinceLastFogHit += Time.deltaTime;
        if (player.timeSinceLastFogHit >= hitInterval)
        {
            player.timeSinceLastFogHit -= hitInterval;
            foreach (AIEnemy aiEnemy in player.currentFogAttackTargets)
            {
            }
            foreach (AIEnemy aiEnemy in player.toRemoveFogAttackTargets)
            {
                player.currentFogAttackTargets.Remove(aiEnemy);
            }
            player.toRemoveFogAttackTargets.Clear();
        }
    }
}
