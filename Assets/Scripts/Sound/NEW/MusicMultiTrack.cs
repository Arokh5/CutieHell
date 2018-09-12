using UnityEngine;

public class MusicMultiTrack : MonoBehaviour
{
    public delegate bool BoolCallback();

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
    [Tooltip("Indicates whether the playFrequency should be used as a randomizing factor or not.")]
    private bool randomizePlay;

    private AudioSource mainAudioSource;
    private AudioSource secondaryAudioSource;

    private BoolCallback endCallback;
    // Gets reset after the first play of the secondary clip (if active)
    private int playCycle = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(mainClip, "ERROR: Main Clip (AudioClip) not assigned for MusicMultiTrack in GameObject '" + gameObject.name + "'!");
        enabled = false;
    }

    private void Update()
    {
        if (!mainAudioSource.isPlaying)
        {
            secondaryAudioSource.Stop();
            Debug.Log("TEST: Music finished!");
            bool shouldContinue = endCallback();
            if (shouldContinue)
            {
                Debug.Log("TEST: Will continue!");
                PlayMusic();
            }
            else
            {
                Debug.Log("TEST: Will stop!");
                Stop();
            }
        }
    }

    private void OnValidate()
    {
        if (firstPlayIndex < 0)
            firstPlayIndex = 0;

        if (playFrequency < 0)
            playFrequency = 0;
    }
    #endregion

    #region Public Methods
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

    public void Play(BoolCallback keepPlayingEndCallback)
    {
        if (IsValid())
        {
            if (!enabled)
            {
                enabled = true;
                endCallback = keepPlayingEndCallback;
                ResetInfo();
                PlayMusic();
            }
            else
            {
                Debug.LogWarning("WARNING: MusicMultiTrack::Play method called while the script is already playing. The call will be ignored!");
            }
        }
        else
        {
            Debug.LogError("ERROR: MusicMultiTrack::Play called in an invalid state. Did you forget to call SetAudioSources prior to calling the Play method?");
        }
    }

    public void Stop()
    {
        enabled = false;
        endCallback = null;
    }
    #endregion

    #region Private Methods
    private bool IsValid()
    {
        return mainAudioSource && (secondaryClip ? secondaryAudioSource : true);
    }

    private void ResetInfo()
    {
        playCycle = -1;
    }

    private void PlayMusic()
    {
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
