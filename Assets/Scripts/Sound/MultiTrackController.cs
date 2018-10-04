using UnityEngine;

public class MultiTrackController : MonoBehaviour
{
    [System.Serializable]
    private class MultiTrackInfo
    {
        public string name;
        public int minHazardLevel;
        public int maxHazardLevel;
        public MusicMultiTrack track;
    }

    [System.Serializable]
    public class MonumentEffect
    {
        public float normalizedDamage;
        public float effectFactor;
    }

    #region Fields
    [Header("Elements setup")]
    [SerializeField]
    private AudioSource mainAudioSource;
    [SerializeField]
    private AudioSource secondaryAudioSource;

    [Header("Tracks setup")]
    [SerializeField]
    private MultiTrackInfo[] trackInfos;

    [Header("Track switching setup")]
    [SerializeField]
    public float generalOffset = 0.0f;
    [SerializeField]
    private MonumentEffect minMonumentEffect;
    [SerializeField]
    private MonumentEffect maxMonumentEffect;

    [Header("Fading out")]
    [SerializeField]
    private float fadeOutTime = 0.5f;

    private float elapsedFadeTime = 0.0f;

    [Header("Information")]
    [SerializeField]
    [ShowOnly]
    private bool isPlaying = false;
    [SerializeField]
    [ShowOnly]
    private bool isFadingOut = false;
    [SerializeField]
    [ShowOnly]
    private int activeTrackIndex = 0;

    private MusicMultiTrack activeMultiTrack;

    [Header("Overriding")]
    public bool overrideNextTrack;
    public int nextTrackIndex = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(mainAudioSource, "ERROR: mainAudioSource (AudioSource) not assigned for MultiTrackController in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(secondaryAudioSource, "ERROR: secondaryAudioSource (AudioSource) not assigned for MultiTrackController in GameObject '" + gameObject.name + "'!");

        for (int i = 0; i < trackInfos.Length; ++i)
        {
            MultiTrackInfo info = trackInfos[i];
            info.track.SetAudioSources(mainAudioSource, secondaryAudioSource);
        }
        ValidateMultiTrackInfos();
    }

    private void Update()
    {
        if (isFadingOut)
        {
            FadeOutEffect();
        }

        if (isPlaying && !mainAudioSource.isPlaying)
        {
            secondaryAudioSource.Stop();
            MusicMultiTrack currentMultiTrack = GetCurrentMultiTrack();

            if (currentMultiTrack == activeMultiTrack)
            {
                activeMultiTrack.ContinuePlaying();
            }
            else
            {
                if (activeMultiTrack != null)
                {
                    activeMultiTrack.Stop();
                }
                currentMultiTrack.StartPlaying();
                activeMultiTrack = currentMultiTrack;
            }
        }
    }

    private void OnValidate()
    {
        foreach (MultiTrackInfo info in trackInfos)
        {
            info.track.OnValidate();
        }
    }
    #endregion

    #region Public Methods
    public void Play()
    {
        isFadingOut = false;
        SetAudioSourcesVolume(1.0f);
        StopAudioSources();
        isPlaying = true;
    }

    public void FadeOut()
    {
        if (isPlaying)
        {
            StopMultiTrack();
            elapsedFadeTime = 0.0f;
            isFadingOut = true;
        }
    }

    public void Stop()
    {
        StopMultiTrack();
        StopAudioSources();
    }
    #endregion

    #region Private Methods
    private void StopMultiTrack()
    {
        if (activeMultiTrack != null)
        {
            activeMultiTrack.Stop();
            activeMultiTrack = null;
        }
        isPlaying = false;
    }

    private MusicMultiTrack GetCurrentMultiTrack()
    {
        if (overrideNextTrack && nextTrackIndex >= 0 && nextTrackIndex < trackInfos.Length)
        {
            activeTrackIndex = nextTrackIndex;
        }
        else if (overrideNextTrack)
        {
            Debug.LogError("ERROR: MultiTrackController override track index set to a value (" + nextTrackIndex + ") out of the range [0, " + trackInfos.Length + "]!");
        }
        else
        {
            float hazardLevel = GetHazardLevel();
            UpdateActiveTrackIndex(hazardLevel);
        }

        Debug.Log("INFO: (MultiTrackController) Playing track " + trackInfos[activeTrackIndex].name);
        return trackInfos[activeTrackIndex].track;
    }

    private void UpdateActiveTrackIndex(float currentHazardLevel)
    {
        int maxIndex = trackInfos.Length - 1;

        bool found = false;
        while (!found)
        {
            MultiTrackInfo info = trackInfos[activeTrackIndex];
            if (currentHazardLevel < info.minHazardLevel && activeTrackIndex > 0)
            {
                --activeTrackIndex;
            }
            else if (currentHazardLevel > info.maxHazardLevel && activeTrackIndex < maxIndex)
            {
                ++activeTrackIndex;
            }
            else
            {
                found = true;
            }
        }
    }

    private float GetHazardLevel()
    {
        // Get game info
        int enemiesCount = GameManager.instance.GetEnemiesCount();
        float normalizedMonumentDamage = 1.0f - GameManager.instance.GetCurrentMonumentNormalizedHealth();

        // Calculate monumentFactor
        float monumentFactor;
        if (normalizedMonumentDamage <= minMonumentEffect.normalizedDamage)
        {
            monumentFactor = minMonumentEffect.effectFactor;
        }
        else if (normalizedMonumentDamage >= maxMonumentEffect.normalizedDamage)
        {
            monumentFactor = maxMonumentEffect.effectFactor;
        }
        else
        {
            // Interpolate linearly in range (y = mx + b)
            float rangeY = maxMonumentEffect.effectFactor - minMonumentEffect.effectFactor;
            float rangeX = maxMonumentEffect.normalizedDamage - minMonumentEffect.normalizedDamage;
            float m = rangeY / rangeX;
            monumentFactor = minMonumentEffect.effectFactor + m * (normalizedMonumentDamage - minMonumentEffect.normalizedDamage);
        }

        // Calculate hazardLevel
        float hazardLevel = generalOffset + enemiesCount * monumentFactor;

        Debug.Log("INFO: (MultiTrackController) Hazard lvl: " + hazardLevel + " (Enemies: " + enemiesCount + " | Mon. Dmg: " + normalizedMonumentDamage + ")");
        return hazardLevel;
    }

    private void FadeOutEffect()
    {
        elapsedFadeTime += Time.unscaledDeltaTime;
        if (elapsedFadeTime > fadeOutTime)
        {
            elapsedFadeTime = fadeOutTime;
        }

        float progress = elapsedFadeTime / fadeOutTime;
        float volume = 1.0f - progress;

        SetAudioSourcesVolume(volume);

        if (elapsedFadeTime == fadeOutTime)
        {
            StopAudioSources();
            isFadingOut = false;
        }
    }

    private void SetAudioSourcesVolume(float volume)
    {
        mainAudioSource.volume = volume;
        secondaryAudioSource.volume = volume;
    }

    private void StopAudioSources()
    {
        mainAudioSource.Stop();
        secondaryAudioSource.Stop();
    }

    private void ValidateMultiTrackInfos()
    {
        if (trackInfos.Length > 0)
        {
            float previousMaxHazard = trackInfos[0].minHazardLevel;

            for (int i = 0; i < trackInfos.Length; ++i)
            {
                MultiTrackInfo info = trackInfos[i];
                info.track.ValidateSetup(gameObject.name + " under the name " + info.name + " (index " + i + ")");
                if (info.minHazardLevel > info.maxHazardLevel)
                {
                    Debug.LogError("ERROR: The maxHazardLevel is lower than the minHazardLevel in MultiTrackController in GameObject '" + gameObject.name + "' for MultiTrackInfo at index " + i + "!");
                }

                if (info.minHazardLevel > previousMaxHazard)
                {
                    // Gap found
                    Debug.LogError("ERROR: There is a gap between hazardLevels in MultiTrackController in GameObject '" + gameObject.name + "' between MultiTrackInfos at indexes " + (i - 1) + " and " + i + "!");
                }
                previousMaxHazard = info.maxHazardLevel;
            }
        }

    }
    #endregion
}
