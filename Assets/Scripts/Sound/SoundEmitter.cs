using UnityEngine;

public abstract class SoundEmitter : MonoBehaviour
{
    #region Fields
    [Header("Audio Source Setup")]
    public AudioClip clip;
    public bool loopAudio;
    public float soundMaxDistance = 10.0f;

    protected AudioSource audioSource;
    protected bool effectOn;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        UnityEngine.Assertions.Assert.IsNotNull(audioSource, "ERROR: SoundEmitter in gameObject '" + gameObject.name + "' could not find an AudioSource in its children hierarchy!");
        UnityEngine.Assertions.Assert.IsNotNull(clip, "ERROR: An AudioClip (clip) has NOT been assigned in SoundEmitter in GameObject '" + gameObject.name + "'!");
        audioSource.clip = clip;
        audioSource.loop = loopAudio;
        audioSource.maxDistance = soundMaxDistance;
    }

    private void OnValidate()
    {
        if (!audioSource)
            audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource)
        {
            audioSource.clip = clip;
            audioSource.loop = loopAudio;
            audioSource.maxDistance = soundMaxDistance;
        }
    }
    #endregion

    #region Public Methods
    public void TriggerOn()
    {
        if (!effectOn)
        {
            effectOn = true;
            PlaySoundEffect();
        }
    }

    public void TriggerOff()
    {
        if (effectOn)
        {
            effectOn = false;
            StopSoundEffect();
        }
    }
    
    #endregion

    #region Protected Methods
    protected abstract void PlaySoundEffect();
    protected abstract void StopSoundEffect();
    #endregion
}
