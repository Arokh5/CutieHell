using UnityEngine;

public class MultiTrackController : MonoBehaviour
{
    [System.Serializable]
    private class MultiTrackInfo
    {
        public string name;
        public MusicMultiTrack track;
    }

    #region Fields
    [Header("Elements setup")]
    [SerializeField]
    private AudioSource mainAudioSource;
    [SerializeField]
    private AudioSource secondaryAudioSource;

    [SerializeField]
    private MultiTrackInfo[] trackInfos;

    private MusicMultiTrack activeMultiTrack;

    [Header("Testing")]
    public bool isPlaying = false;
    public bool start = false;
    public bool useAlternate = false;
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
            info.track.ValidateSetup(gameObject.name + " under the name " + info.name + " (index " + i + ")");
        }

        activeMultiTrack = trackInfos[0].track;
    }

    private void Update()
    {
        if (isPlaying && !mainAudioSource.isPlaying)
        {
            secondaryAudioSource.Stop();
            Debug.Log("TEST: Music finished!");
            MusicMultiTrack nextMultiTrack = GetAdequateMultiTrack();

            if (nextMultiTrack == activeMultiTrack)
            {
                Debug.Log("TEST: Will continue!");
                activeMultiTrack.ContinuePlaying();
            }
            else
            {
                Debug.Log("TEST: Will stop!");
                activeMultiTrack.Stop();
                nextMultiTrack.StartPlaying();
                activeMultiTrack = nextMultiTrack;
            }
        }

        if (start)
        {
            start = false;
            activeMultiTrack.StartPlaying();
            isPlaying = true;
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

    #region Private Methods
    private MusicMultiTrack GetAdequateMultiTrack()
    {
        Debug.Log("TEST: Queried!");
        if (useAlternate)
            return trackInfos[1].track;
        else
            return trackInfos[0].track;
    }
    #endregion
}
