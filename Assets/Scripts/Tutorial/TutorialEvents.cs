using UnityEngine;

public class TutorialEvents: MonoBehaviour
{
    private delegate void TutorialEvent();

    #region Fields
    [Header("DropLighting")]
    public ParticleSystem lightingPrefab;
    public Transform lightingPosition;
    public MonumentIndicator monumentIndicator;

    private TutorialEvent[] events;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        events = new TutorialEvent[1];
        events[0] = DropLighting;
    }
    #endregion

    #region Public Methods
    public void LaunchEvent(int eventIndex)
    {
        if (eventIndex >= 0 && eventIndex < events.Length)
            events[eventIndex]();
    }
    #endregion

    #region Private Methods
    private void DropLighting()
    {
        ParticlesManager.instance.LaunchParticleSystem(lightingPrefab, lightingPosition.position, lightingPrefab.transform.rotation);
        monumentIndicator.RequestOpen();
    }
    #endregion
}
