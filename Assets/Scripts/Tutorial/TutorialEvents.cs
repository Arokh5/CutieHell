using UnityEngine;

public class TutorialEvents: MonoBehaviour
{
    private delegate void TutorialEvent();

    [System.Serializable]
    private class EnemyLabelInfo
    {
        public string name;
        public string description;
    }

    #region Fields
    [Header("UI")]
    [SerializeField]
    private GameObject tutObjectiveMarker;
    [SerializeField]
    private GameObject tutObjectiveIcon;
    [SerializeField]
    private GameObject[] bannersAndMarkers;
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private GameObject evilGainInfoText;

    [Header("General")]
    [SerializeField]
    private Cinemachine.CinemachineBrain cmBrain;
    public AISpawner tutorialSpawner;
    public AISpawner tutorialSpawner2;
    public AISpawner zoneDSpawner;
    [SerializeField]
    private DamageLimiter damageLimiterTutZone;
    public EnemyDescriptionController enemyDescriptionController;
    [SerializeField]
    private EnemyLabelInfo[] enemyLabelInfos;

    [Header("Info Prompts")]
    public InformationPromptController infoPromptController;
    [SerializeField]
    private string[] infoPrompts;

    [Header("Events to activate")]
    [SerializeField]
    private GameObject[] enemyCountMonitors;

    [Header("0-DropLighting")]
    public ParticleSystem lightingPrefab;
    public Transform lightingPosition;
    public MonumentIndicator monumentIndicator;

    private TutorialController tutorialController;
    private TutorialEvent[] events;
    private TutorialEnemiesManager tutorialEnemiesManager;
    private AIEnemy firstConqueror;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        tutorialController = GetComponent<TutorialController>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialController, "ERROR: A TutorialController Component could not be found by TutorialEvents in GameObject " + gameObject.name);
        evilGainInfoText.SetActive(false);

        events = new TutorialEvent[]{
            DropLighting,           // 00
            SpawnSlime,             // 01
            SlimeAttack,            // 02
            SpawnBear,              // 03
            BearAttack,             // 04
            SpawnConqueror,         // 05
            ConquerorAttack,        // 06
            HaltEnemies,            // 07
            RestoreUI,              // 08
            DisableCMBrain,         // 09
            PlayerMoveLesson,       // 10
            HideInfoPrompt,         // 11
            PlayerTeleportLesson,   // 12
            Teleported,             // 13
            EnterZoneBtoCBridge,    // 14
            EnemyCountTo1,          // 15
            EnemyCountTo0           // 16
        };
    }
    #endregion

    #region Public Methods
    public void SetTutorialEnemiesManager(TutorialEnemiesManager tutorialEnemiesManager)
    {
        this.tutorialEnemiesManager = tutorialEnemiesManager;
    }

    public int GetEnemiesCount()
    {
        return tutorialEnemiesManager.GetEnemiesCount();
    }

    public void OnTutorialStarted()
    {
        damageLimiterTutZone.gameObject.SetActive(true);

        foreach (GameObject go in bannersAndMarkers)
            go.SetActive(false);

        foreach (GameObject go in enemyCountMonitors)
            go.SetActive(false);

        tutObjectiveIcon.SetActive(true);
        tutObjectiveMarker.SetActive(false);
        crosshair.SetActive(false);
    }

    public void OnTutorialEnded()
    {
        damageLimiterTutZone.gameObject.SetActive(false);

        RestoreUI();
    }

    public void LaunchEvent(int eventIndex)
    {
        if (eventIndex >= 0 && eventIndex < events.Length)
        {
            Debug.Log("Launching event with index " + eventIndex);
            events[eventIndex]();
        }
        else
            Debug.LogError("ERROR: eventIndex parameter (" + eventIndex + ") out of range in TutorialEvents::LaunchEvent in gameObject '" + gameObject.name + "'!");
    }
    #endregion

    #region Private Methods
    #region Events
    // 00
    private void DropLighting()
    {
        ParticlesManager.instance.LaunchParticleSystem(lightingPrefab, lightingPosition.position, lightingPrefab.transform.rotation);
        monumentIndicator.RequestOpen();
        tutObjectiveMarker.SetActive(false);
    }

    // 01
    private void SpawnSlime()
    {
        AIEnemy slime = SpawnEnemy(tutorialSpawner, EnemyType.BASIC);
        tutorialEnemiesManager.AddEnemy(slime);
        slime.agent.enabled = false;
        SetEnemyLabelInfo(0);
        damageLimiterTutZone.normalizedMaxDamage = 0.2f;
    }

    // 02
    private void SlimeAttack()
    {
        tutorialEnemiesManager.ResumeEnemies();
    }

    // 03
    private void SpawnBear()
    {
        AIEnemy bear = SpawnEnemy(tutorialSpawner2, EnemyType.RANGE);
        tutorialEnemiesManager.AddEnemy(bear);
        bear.agent.enabled = false;
        SetEnemyLabelInfo(1);
        damageLimiterTutZone.normalizedMaxDamage = 0.5f;
    }

    // 04
    private void BearAttack()
    {
        tutorialEnemiesManager.ResumeEnemies();
    }

    // 05
    private void SpawnConqueror()
    {
        firstConqueror = SpawnEnemy(tutorialSpawner, EnemyType.CONQUEROR);
        // Not added to the tutorialenemiesManager because the conqueror will conquer and remain in ZoneD
        firstConqueror.agent.enabled = false;
        SetEnemyLabelInfo(2);
        damageLimiterTutZone.normalizedMaxDamage = 1.1f;
    }

    // 06
    private void ConquerorAttack()
    {
        firstConqueror.agent.enabled = true;
    }
       
    // 07
    private void HaltEnemies()
    {
        tutorialEnemiesManager.HaltEnemies();
    }

    // 08
    private void RestoreUI()
    {
        foreach (GameObject go in bannersAndMarkers)
            go.SetActive(true);

        tutObjectiveIcon.SetActive(false);
        tutObjectiveMarker.SetActive(false);
        crosshair.SetActive(true);
    }

    // 09
    private void DisableCMBrain()
    {
        cmBrain.enabled = false;
    }

    // 10
    private void PlayerMoveLesson()
    {
        tutorialController.NextPlayerState();
        infoPromptController.ShowPrompt(infoPrompts[0]);
    }

    // 11
    private void HideInfoPrompt()
    {
        infoPromptController.HidePrompt();
    }

    // 12
    private void PlayerTeleportLesson()
    {
        tutorialController.NextPlayerState();
        infoPromptController.ShowPrompt(infoPrompts[1]);
    }

    // 13
    private void Teleported()
    {
        infoPromptController.ShowPrompt(infoPrompts[2]);
    }

    // 14
    private void EnterZoneBtoCBridge()
    {
        infoPromptController.ShowPrompt(infoPrompts[3]);
        tutorialEnemiesManager.ResumeEnemies();
        tutorialController.NextPlayerState();
        foreach (GameObject go in enemyCountMonitors)
            go.SetActive(true);
    }

    // 15
    private void EnemyCountTo1()
    {
        evilGainInfoText.SetActive(true);
    }

    // 16
    private void EnemyCountTo0()
    {
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialController.NextPlayerState();
        infoPromptController.ShowPrompt(infoPrompts[4]);
    }


    #endregion

    private AIEnemy SpawnEnemy(AISpawner spawner, EnemyType enemyType)
    {
        AIEnemy enemy = spawner.SpawnOne(enemyType);
        return enemy;
    }

    private void SetEnemyLabelInfo(int infoIndex)
    {
        if (infoIndex >= 0 && infoIndex < enemyLabelInfos.Length)
        {
            enemyDescriptionController.SetName(enemyLabelInfos[infoIndex].name);
            enemyDescriptionController.SetDescription(enemyLabelInfos[infoIndex].description);
        }
        else
        {
            Debug.LogError("ERROR: infoIndex paremeter out of range in TutorialEvents::SetEnemyLabelInfo in gameObject '" + gameObject.name + "'!");
        }
    }
    #endregion
}
