using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/FogEnter")]
public class FogEnter : StateAction
{
    public override void Act(Player player)
    {
        player.fogStateLastTime = Time.time;
        player.SetRenderersVisibility(false);
        player.fogCollider.enabled = true;
        player.fogVFX.gameObject.SetActive(true);
        player.SetIsAutoRecoveringEvil(false);
    }
}
