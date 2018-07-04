using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/HealthRecovery")]
public class HealthRecovery : StateAction
{
    public override void Act(Player player)
    {
        if (player.timeSinceLastHit > player.autoHealDelay)
        {
            float healthToIncrease = Time.deltaTime * player.GetMaxHealth() / player.fullAutoHealDuration;
            float normalizedTargetHealth = (player.GetCurrentHealth() + healthToIncrease) / player.GetMaxHealth();
            if (normalizedTargetHealth > 1.0f)
                normalizedTargetHealth = 1.0f;

            player.SetCurrentHealth(normalizedTargetHealth);
        }
        else
        {
            player.timeSinceLastHit += Time.deltaTime;
        }
    }
}
