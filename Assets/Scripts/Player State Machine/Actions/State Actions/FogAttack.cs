using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/FogAttack")]
public class FogAttack : StateAction
{
    public float evilCostPerSecond;
    public float hitInterval;
    public int dps;
    
    public override void Act(Player player)
    {
        player.accumulatedFogEvilCost += evilCostPerSecond * Time.deltaTime;
        if (player.accumulatedFogEvilCost > 1.0f)
        {
            player.accumulatedFogEvilCost -= 1.0f;
            player.AddEvilPoints(-1);
        }

        player.timeSinceLastFogHit += Time.deltaTime;
        if (player.timeSinceLastFogHit >= hitInterval)
        {
            player.timeSinceLastFogHit -= hitInterval;
            foreach (AIEnemy aiEnemy in player.currentFogAttackTargets)
            {
                aiEnemy.TakeDamage(dps * hitInterval, AttackType.FOG);
            }
        }
    }
}
