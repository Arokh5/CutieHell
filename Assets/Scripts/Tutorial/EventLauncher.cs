using UnityEngine;

public class EventLauncher : MonoBehaviour
{
    #region Fields
    public int eventIndex = -1;
    private TutorialController tutorialController;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        tutorialController = GetComponentInParent<TutorialController>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialController, "ERROR: A TutorialController Component could not be found in the parent hierarchy by EventLauncher in GameObject " + gameObject.name);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (tutorialController)
            tutorialController.LaunchEvent(eventIndex);
    }
    #endregion
}
