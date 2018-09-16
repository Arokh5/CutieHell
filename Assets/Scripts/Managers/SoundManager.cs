using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private class UsageInfo
    {
        private List<float> timers = new List<float>();

        public void UpdateTimers(float deltaTime)
        {
            for (int i = timers.Count - 1; i >= 0 ; --i)
            {
                float timer = timers[i];
                timer -= deltaTime;
                timers[i] = timer;
                if (timer <= 0)
                    timers.RemoveAt(i);
            }
        }

        public void AddTimer(float timerStart)
        {
            if (timerStart > 0)
                timers.Add(timerStart);
            else
                Debug.LogWarning("WARNING: SoundManager::UsageInfo::AddTimer called with a negative value for timerStart. No timer will be started!");
        }

        public int GetCount()
        {
            return timers.Count;
        }
    }

    #region Fields
    public static SoundManager instance;

    [Header("Sources setup")]
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource ambienceSource;
    [SerializeField]
    private AudioSource sfxSource;

    [Header("SFX Repetition limits")]
    [SerializeField]
    [Tooltip("The time (in seconds) after playback during which a clip is considered to be 'active'")]
    private float activeSoundTime = 0.2f;
    [SerializeField]
    [Tooltip("The maximum number of 'active' instance of a clip that are allowed to play")]
    private int maxRepeats = 2;
    
    private Dictionary<AudioClip, UsageInfo> activeClips = new Dictionary<AudioClip, UsageInfo>();

    [Header("Music Multi Tracks")]
    [SerializeField]
    private MultiTrackController multiTrackController;
	#endregion
	
	#region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        UnityEngine.Assertions.Assert.IsNotNull(musicSource, "ERROR: Music Source (AudioSource) not assigned for SoundManager in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(ambienceSource, "ERROR: Ambient Source (AudioSource) not assigned for SoundManager in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(sfxSource, "ERROR: Sfx Source (AudioSource) not assigned for SoundManager in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(sfxSource, "ERROR: Multi Track Controller (MultiTrackController) not assigned for SoundManager in GameObject '" + gameObject.name + "'!");

    }

    private void Start()
    {
        multiTrackController.Play();
    }

    private void Update()
    {
        foreach (UsageInfo usageInfo in activeClips.Values)
        {
            usageInfo.UpdateTimers(Time.unscaledDeltaTime);
        }
    }

    private void OnValidate()
    {
        if (activeSoundTime < 0.0f)
            activeSoundTime = 0.0f;

        if (maxRepeats < 1)
            maxRepeats = 1;
    }
    #endregion

    #region Public Methods
    public void PlayMusicClip(AudioClip musicClip, bool loop = false, float pitch = 1f)
    {
        multiTrackController.FadeOut();
        musicSource.clip = musicClip;
        musicSource.pitch = pitch;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void RestoreDefaultBGM()
    {
        musicSource.Stop();
        multiTrackController.Play();
    }

    public void PlaySfxClip(AudioClip sfxClip, float pitch = 1f)
    {
        if (sfxClip != null && CanPlay(sfxClip))
        {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(sfxClip);
        }
    }

    public void PlaySfxClip(AudioSource source, AudioClip clip = null, bool oneShot = false)
    {
        if (source != null)
        {
            if (CanPlay(clip ? clip : source.clip))
            {
                if (oneShot)
                {
                    if (clip)
                        source.PlayOneShot(clip);
                    else
                        source.PlayOneShot(source.clip);
                }
                else
                {
                    if (clip)
                        source.clip = clip;

                    source.Play();
                }
            }
        }
        else
        {
            Debug.LogWarning("WARNING: SoundManager::PlaySfxClip called with a source (AudioSource) value of null!");
        }
    }
    #endregion

    #region Private Methods
    private bool CanPlay(AudioClip clip)
    {
        bool canPlay = true;

        if (clip != null)
        {
            UsageInfo usageInfo;
            if (activeClips.TryGetValue(clip, out usageInfo))
            {
                if (usageInfo.GetCount() < maxRepeats)
                {
                    usageInfo.AddTimer(activeSoundTime);
                }
                else
                {
                    canPlay = false;
                }
            }
            else
            {
                usageInfo = new UsageInfo();
                usageInfo.AddTimer(activeSoundTime);
                activeClips.Add(clip, usageInfo);
            }
        }
        else
        {
            canPlay = false;
        }

        return canPlay;
    }
    #endregion
}
