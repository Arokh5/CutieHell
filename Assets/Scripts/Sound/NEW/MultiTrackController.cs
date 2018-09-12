using UnityEngine;

public class MultiTrackController : MonoBehaviour
{
    #region Fields
    [Header("Elements setup")]
    [SerializeField]
    private AudioSource mainAudioSource;
    [SerializeField]
    private AudioSource secondaryAudioSource;

    [Header("Testing")]
    public MusicMultiTrack testTrack;
    public bool start = false;
    public bool shouldContinue = true;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(mainAudioSource, "ERROR: mainAudioSource (AudioSource) not assigned for MultiTrackController in GameObject '" + gameObject.name + "'!");
        UnityEngine.Assertions.Assert.IsNotNull(secondaryAudioSource, "ERROR: secondaryAudioSource (AudioSource) not assigned for MultiTrackController in GameObject '" + gameObject.name + "'!");
    }

    private void Start()
    {
        testTrack.SetAudioSources(mainAudioSource, secondaryAudioSource);
    }

    private void Update()
    {
        if (start)
        {
            start = false;
            testTrack.Play(ShouldContinuePlaying);
        }
    }
    #endregion

    #region Public Methods
    bool ShouldContinuePlaying()
    {
        Debug.Log("TEST: Question asked");
        return shouldContinue;
    }
    #endregion
}
