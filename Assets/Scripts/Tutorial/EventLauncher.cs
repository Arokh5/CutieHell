using UnityEngine;

public class EventLauncher : MonoBehaviour
{
    private enum LauncherType
    {
        ACTIVATION,
        TRIGGER_ENTER,
        TRIGGER_EXIT
    }

    #region Fields
    [SerializeField]
    private LauncherType type;
    public int eventIndex = -1;
    public bool deactivateOnLaunch = false;
    public LayerMask triggerLayerMask;

    private TutorialController tutorialController;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        tutorialController = GetComponentInParent<TutorialController>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialController, "ERROR: A TutorialController Component could not be found in the parent hierarchy by EventLauncher in GameObject " + gameObject.name);
    }

    private void OnEnable()
    {
        if (type == LauncherType.ACTIVATION && tutorialController)
            LaunchEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == LauncherType.TRIGGER_ENTER && tutorialController)
        {
            if (triggerLayerMask.value == other.gameObject.layer)
            {
                LaunchEvent();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (type == LauncherType.TRIGGER_EXIT && tutorialController)
        {
            if (((1 << other.gameObject.layer) & triggerLayerMask) != 0)
            {
                LaunchEvent();
            }
        }
    }
    #endregion

    #region Private Moethods
    private void LaunchEvent()
    {
        tutorialController.LaunchEvent(eventIndex);
        if (deactivateOnLaunch)
            gameObject.SetActive(false);
    }
    #endregion
}
