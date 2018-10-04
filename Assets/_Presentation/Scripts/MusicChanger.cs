using UnityEngine;

public class MusicChanger : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("The Multi Track Controller that handles music playback")]
    private MultiTrackController multiTrackController;
    [SerializeField]
    [Tooltip("The music track index to change to when the Reference Zone gets conquered")]
    private int musicTrackIndex = -1;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for MusicChanger script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(multiTrackController, "ERROR: Multi Track Controller (MultiTrackController) not assigned for MusicChanger script in GameObject " + gameObject.name);
    }

    private void Start()
    {
        referenceZone.AddIZoneTakenListener(this);
    }

    private void OnDestroy()
    {
        referenceZone.RemoveIZoneTakenListener(this);
    }
    #endregion

    #region Public Methods
    // IZoneTakenListener
    public void OnZoneTaken()
    {
        multiTrackController.nextTrackIndex = musicTrackIndex;
    }
    #endregion
}
