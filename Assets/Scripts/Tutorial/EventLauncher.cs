using UnityEngine;

public class EventLauncher : MonoBehaviour
{
    private enum LauncherType
    {
        ACTIVATION,
        TRIGGER_ENTER,
        TRIGGER_EXIT,
        ENEMY_COUNT
    }

    #region Fields
    [SerializeField]
    private LauncherType type;
    public int eventIndex = -1;
    public bool deactivateOnLaunch = false;

    [Header("Trigger")]
    public LayerMask triggerLayerMask;

    [Header("Enemy Count")]
    public int enemyCount = -1;
    private bool enemyCountReached = false;

    private TutorialController tutorialController;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        tutorialController = GetComponentInParent<TutorialController>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialController, "ERROR: A TutorialController Component could not be found in the parent hierarchy by EventLauncher in GameObject " + gameObject.name);
    }

    private void Update()
    {
        if (type == LauncherType.ENEMY_COUNT)
        {
            int currentEnemyCount = tutorialController.GetEnemiesCount();
            if (!enemyCountReached && currentEnemyCount == enemyCount)
            {
                enemyCountReached = true;
                LaunchEvent();
            }
            else if (enemyCountReached && currentEnemyCount != enemyCount)
            {
                enemyCountReached = false;
            }
        }
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
            if (Helpers.GameObjectInLayerMask(other.gameObject, triggerLayerMask))
            {
                LaunchEvent();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (type == LauncherType.TRIGGER_EXIT && tutorialController)
        {
            if (Helpers.GameObjectInLayerMask(other.gameObject, triggerLayerMask))
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
