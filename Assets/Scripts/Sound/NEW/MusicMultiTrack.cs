using UnityEngine;

[System.Serializable]
public class MusicMultiTrack
{
    #region Fields
    [SerializeField]
    private AudioClip mainClip;

    [Header("Secondary Clip")]
    [SerializeField]
    [Tooltip("If left empty, only the mainClip will be played.")]
    private AudioClip secondaryClip;
    [SerializeField]
    [Tooltip("Defines the first time that the secondaryClip gets played. A value of 0 indicates that it is first played the first time that the mainClip is played." +
        " A value of 1 indicates that it first gets played on the second time the mainClip is played. Etc.")]
    private int firstPlayIndex;
    [SerializeField]
    [Tooltip("Determines how often the seconday clip gets played after its first play. A value of 0 indicates that it never gets played again. " +
        "A value of one indicates that it gets played everytime the mainClip is played. A value of 2 indicates that it gets played every second time the mainClip is played.")]
    private int playFrequency;
    [SerializeField]
    [Tooltip("Indicates whether playback of the secondary Clip is decided randomly. In this case, the playFrequency is used as a randomizing factor. A playFrequency of 4 measn 25% (1/4) chances of playback.")]
    private bool randomizePlay;

    [SerializeField]
    [ShowOnly]
    private bool isPlaying;

    private AudioSource mainAudioSource;
    private AudioSource secondaryAudioSource;
    // Gets reset after the first play of the secondary clip (if active)
    private int playCycle = -1;
    #endregion

    #region Public Methods
    public void OnValidate()
    {
        if (firstPlayIndex < 0)
            firstPlayIndex = 0;

        if (playFrequency < 0)
            playFrequency = 0;
    }

    public void ValidateSetup(string hostName)
    {
        UnityEngine.Assertions.Assert.IsNotNull(mainClip, "ERROR: Main Clip (AudioClip) not assigned for MusicMultiTrack hosted in " + hostName + "!");
        UnityEngine.Assertions.Assert.IsNotNull(mainAudioSource, "ERROR: mainAudioSource is null in MusicMultiTrack hosted in " + hostName + ". Did you forget to call SetAudioSources before validating the setup?");
        UnityEngine.Assertions.Assert.IsTrue(!secondaryClip || secondaryAudioSource, "ERROR: secondaryAudioSource is null in MusicMultiTrack script hosted in " + hostName + ". Did you forget to call SetAudioSources before validating the setup?");
    }

    public void SetAudioSources(AudioSource mainAudioSource, AudioSource secondaryAudioSource)
    {
        this.mainAudioSource = mainAudioSource;
        this.secondaryAudioSource = secondaryAudioSource;
        if (!this.mainAudioSource)
        {
            Debug.LogError("ERROR: The mainAudioSource passed to the MusicMultiTrack::SetAudioSource method is null. The MusicMultiTrack is left in an invalid state and won't be usable.");
        }
        if (!this.secondaryAudioSource && secondaryClip)
        {
            Debug.LogError("ERROR: The secondaryAudioSource passed to the MusicMultiTrack::SetAudioSource method is null and there is a seoncdaryClip that needs it. The MusicMultiTrack is left in an invalid state and won't be usable.");
        }
    }

    public void StartPlaying()
    {
        if (!isPlaying)
        {
            ContinuePlaying();
        }
        else
        {
            Debug.LogWarning("WARNING: MusicMultiTrack::StartPlaying method called while the script is already playing. The MusicMultiTrack will be reset before playback!");
            Stop();
            ContinuePlaying();
        }
    }

    public void ContinuePlaying()
    {
        isPlaying = true;
        ++playCycle;
        mainAudioSource.PlayOneShot(mainClip);

        if (secondaryClip)
        {
            if (playCycle == firstPlayIndex)
            {
                // First time the secondary clip gets played.
                secondaryAudioSource.PlayOneShot(secondaryClip);
            }
            else if (playCycle > firstPlayIndex && playFrequency > 0)
            {
                if (randomizePlay)
                {
                    RandomizedSecondaryPlay();
                }
                else
                {
                    NormalSecondaryPlay();
                }
            }
        }
    }

    public void Stop()
    {
        playCycle = -1;
        isPlaying = false;
    }
    #endregion

    #region Private Methods
    private void NormalSecondaryPlay()
    {
        int repeatsCycle = playCycle - firstPlayIndex;
        if (repeatsCycle % playFrequency == 0)
        {
            secondaryAudioSource.PlayOneShot(secondaryClip);
        }
    }

    private void RandomizedSecondaryPlay()
    {
        float chanceToPlay = 1 / playFrequency;
        float randomNum = Random.Range(0.0f, 1.0f);
        if (randomNum <= chanceToPlay)
        {
            secondaryAudioSource.PlayOneShot(secondaryClip);
        }
    }
    #endregion
}
