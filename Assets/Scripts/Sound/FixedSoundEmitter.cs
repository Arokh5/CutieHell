public class FixedSoundEmitter : SoundEmitter
{
    #region Protected Methods
    protected override void PlaySoundEffect()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    protected override void StopSoundEffect()
    {
        audioSource.Stop();
    }
    #endregion
}
