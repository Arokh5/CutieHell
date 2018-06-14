using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/FogExit")]
public class FogExit : StateAction
{
    public override void Act(Player player)
    {
        player.fogStateLastTime = Time.time;
        player.SetRenderersVisibility(true);
        player.fogCollider.enabled = false;
        player.fogVFX.Stop();
        player.fogVFX.gameObject.SetActive(false);
        player.currentFogAttackTargets.Clear();
        player.accumulatedFogEvilCost = 0.0f;
        player.timeSinceLastFogHit = 0.0f;
        player.SetIsAutoRecoveringEvil(true);
    }
}
