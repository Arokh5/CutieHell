using UnityEngine;

public abstract class TutorialEvents : MonoBehaviour
{
    #region Public Methods
    public abstract void SetTutorialEnemiesManager(TutorialEnemiesManager tutorialEnemiesManager);
    public abstract void OnTutorialStarted();
    public abstract void OnTutorialEnded();
    public abstract void LaunchEvent(int eventIndex);
    #endregion
}
