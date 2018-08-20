using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlaySound")]
public class PlaySound : StateAction
{
    public AudioClip audioClip;

    public override void Act(Player player)
    {
        SoundManager.instance.PlaySfxClip(player.oneShotAudioSource, audioClip, true);
    }
}