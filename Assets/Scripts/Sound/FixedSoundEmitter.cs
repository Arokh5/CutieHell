public class FixedSoundEmitter : SoundEmitter
{
    #region Fields
    public bool restartOnRetrigger;
    #endregion

    #region Protected Methods
    protected override void PlaySoundEffect()
    {
        if (restartOnRetrigger || !audioSource.isPlaying)
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
