using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/FogEnter")]
public class FogEnter : StateAction
{
    public AudioClip loopingAudioClip;

    public override void Act(Player player)
    {
        player.fogStateLastTime = Time.time;
        player.SetRenderersVisibility(false);
        player.fogCollider.enabled = true;
        player.fogVFX.gameObject.SetActive(true);
        player.SetIsAutoRecoveringEvil(false);

        player.oneShotAudioSource.loop = true;
        player.oneShotAudioSource.clip = loopingAudioClip;
        player.oneShotAudioSource.Play();
    }
}
