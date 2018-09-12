using UnityEngine;

public class MultiTrackController : MonoBehaviour
{
    #region Fields
    [Header("Elements setup")]
    [SerializeField]
    private AudioSource mainAudioSource;
    [SerializeField]
    private AudioSource secondaryAudioSource;

    private MusicMultiTrack activeMultiTrack;

    [Header("Testing")]
    public MusicMultiTrack testTrack;
    public MusicMultiTrack alternateTrack;
    public bool isPlaying = false;
    public bool start = false;
    public bool useAlternate = false;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(mainAudioSource, "ERROR: mainAudioSource (AudioSource) not assigned for MultiTrackController in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(secondaryAudioSource, "ERROR: secondaryAudioSource (AudioSource) not assigned for MultiTrackController in GameObject '" + gameObject.name + "'!");

        testTrack.SetAudioSources(mainAudioSource, secondaryAudioSource);
        testTrack.ValidateSetup(gameObject.name);
        alternateTrack.SetAudioSources(mainAudioSource, secondaryAudioSource);
        alternateTrack.ValidateSetup(gameObject.name);

        activeMultiTrack = testTrack;
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
        testTrack.OnValidate();
    }
    #endregion

    #region Private Methods
    private MusicMultiTrack GetAdequateMultiTrack()
    {
        Debug.Log("TEST: Queried!");
        if (useAlternate)
            return alternateTrack;
        else
            return testTrack;
    }
    #endregion
}
