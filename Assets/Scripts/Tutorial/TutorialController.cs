using UnityEngine;

public abstract class TutorialController : MonoBehaviour
{
    #region Public Methods
    public abstract void PauseTutorial(bool pause);

    public abstract bool IsRunning();

    public abstract void RequestStartTutorial();

    public abstract void RequestBypassTutorial();

    public abstract void RequestEndTutorial();

    public abstract void LaunchEvent(int eventIndex);

    public abstract void NextPlayerState();

    public abstract int GetEnemiesCount();

    public abstract void PauseTimelineAndReleaseCamera();

    public abstract void ResumeTimelineAndCaptureCamera();
    #endregion
}