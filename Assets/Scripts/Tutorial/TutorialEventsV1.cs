using UnityEngine;

public class TutorialEventsV1 : TutorialEvents
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
    private GameObject objectiveBanners;
    [SerializeField]
    private GameObject objectiveMarkers;
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private GameObject evilGainInfoText;
    [SerializeField]
    private GameObject evilMisuseInfoText;

    [Header("General")]
    [SerializeField]
    private Cinemachine.CinemachineBrain cmBrain;
    public AISpawner tutorialSpawner;
    public AISpawner tutorialSpawner2;
    public AISpawner zoneDSpawner;
    [SerializeField]
    private DamageLimiter damageLimiterTutZone;
    [SerializeField]
    private DamageLimiter damageLimiterZoneC;
    [SerializeField]
    private EvilLimiter evilLimiter;
    [SerializeField]
    private AudioSource audioSource;
    public EnemyDescriptionController enemyDescriptionController;
    [SerializeField]
    private EnemyLabelInfo[] enemyLabelInfos;

    [Header("Info Prompts")]
    public InformationPromptController infoPromptController;
    [SerializeField]
    private string[] infoPrompts;

    [Header("0-DropLightning")]
    [SerializeField]
    private ParticleSystem lightningPrefab;
    [SerializeField]
    private Transform lightningPosition;
    [SerializeField]
    private MonumentIndicator monumentIndicator;
    [SerializeField]
    private AudioClip lightningSFX;

    [Header("14-EnterZoneBtoCBridge")]
    [SerializeField]
    private GameObject[] enemyCountMonitors;

    [Header("16-EnemyCountTo0")]
    [SerializeField]
    private GameObject strongWaveEnemyCountMonitor;

    [Header("17-ZoneTrapWave")]
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera playerAlignedCamera;

    [Header("18-RepositionPlayer")]
    [SerializeField]
    private Transform zoneAtkPlayerMovePos;
    [SerializeField]
    private Transform zoneAtkCameraPos;

    [Header("20-ZoneAttackInfo")]
    [SerializeField]
    private GameObject zoneAttackInfoText;

    [Header("21-PlayerZoneTrapLesson")]
    [SerializeField]
    private GameObject zoneTrapWaveEnemyCountMonitor;

    private TutorialController tutorialController;
    private TutorialEvent[] events;
    private TutorialEnemiesManager tutorialEnemiesManager;
    private AIEnemy firstConqueror;
    private Player player;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        tutorialController = GetComponent<TutorialController>();
        UnityEngine.Assertions.Assert.IsNotNull(tutorialController, "ERROR: A TutorialController Component could not be found by TutorialEventsV1 in GameObject " + gameObject.name);
        evilGainInfoText.SetActive(false);

        events = new TutorialEvent[]{
            DropLightning,          // 00
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
            EnemyCountTo0,          // 16
            StrongAttackWaveKilled, // 17
            RepositionPlayer,       // 18
            SpawnZoneAtkWave,       // 19
            ZoneAttackInfo,         // 20
            PlayerZoneTrapLesson,   // 21   
            ZoneTrapWaveKilled      // 22
        };
    }

    private void Start()
    {
        player = GameManager.instance.GetPlayer1();
    }
    #endregion

    #region Public Methods
    public override void SetTutorialEnemiesManager(TutorialEnemiesManager tutorialEnemiesManager)
    {
        this.tutorialEnemiesManager = tutorialEnemiesManager;
    }

    public override void OnTutorialWillStart()
    {
        damageLimiterTutZone.gameObject.SetActive(true);
        damageLimiterZoneC.gameObject.SetActive(true);
        evilLimiter.gameObject.SetActive(false);
        evilLimiter.RegisterCallback(EvilLimitReached);

        objectiveMarkers.SetActive(false);
        objectiveBanners.SetActive(false);

        foreach (GameObject go in enemyCountMonitors)
            go.SetActive(false);

        strongWaveEnemyCountMonitor.SetActive(false);
        zoneAttackInfoText.SetActive(false);
        zoneTrapWaveEnemyCountMonitor.SetActive(false);

        tutObjectiveIcon.SetActive(true);
        tutObjectiveMarker.SetActive(false);
        crosshair.SetActive(false);
    }

    public override void OnTutorialEnded()
    {
        damageLimiterTutZone.gameObject.SetActive(false);
        damageLimiterZoneC.gameObject.SetActive(false);
        evilLimiter.gameObject.SetActive(false);
        RestoreUI();
    }

    public override void LaunchEvent(int eventIndex)
    {
        if (eventIndex >= 0 && eventIndex < events.Length)
        {
            //Debug.Log("Launching event with index " + eventIndex);
            events[eventIndex]();
        }
        else
            Debug.LogError("ERROR: eventIndex parameter (" + eventIndex + ") out of range in TutorialEventsV1::LaunchEvent in gameObject '" + gameObject.name + "'!");
    }
    #endregion

    #region Private Methods
    #region Events
    // 00
    private void DropLightning()
    {
        ParticlesManager.instance.LaunchParticleSystem(lightningPrefab, lightningPosition.position, lightningPrefab.transform.rotation);
        monumentIndicator.RequestOpen();
        audioSource.PlayOneShot(lightningSFX);
        tutObjectiveMarker.SetActive(false);
    }

    // 01
    private void SpawnSlime()
    {
        AIEnemy slime = SpawnEnemy(tutorialSpawner, EnemyType.BASIC);
        tutorialEnemiesManager.AddEnemy(slime);
        slime.SetAgentEnable(false);
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
        bear.SetAgentEnable(false);
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
        firstConqueror.SetAgentEnable(false);
        SetEnemyLabelInfo(2);
        damageLimiterTutZone.normalizedMaxDamage = 1.1f;
    }

    // 06
    private void ConquerorAttack()
    {
        firstConqueror.SetAgentEnable(true);
    }

    // 07
    private void HaltEnemies()
    {
        tutorialEnemiesManager.HaltEnemies();
    }

    // 08
    private void RestoreUI()
    {
        objectiveMarkers.SetActive(true);
        objectiveBanners.SetActive(true);
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
        tutorialController.PauseTimelineAndReleaseCamera();
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
        // Enemies spawned so that the player uses the strong attack
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialController.NextPlayerState();
        infoPromptController.ShowPrompt(infoPrompts[4]);
        strongWaveEnemyCountMonitor.SetActive(true);
        evilLimiter.gameObject.SetActive(true);
    }

    // 17
    private void StrongAttackWaveKilled()
    {
        evilLimiter.gameObject.SetActive(false);
        tutorialController.NextPlayerState();
        playerAlignedCamera.transform.position = Camera.main.transform.position;
        playerAlignedCamera.transform.rotation = Camera.main.transform.rotation;
        infoPromptController.HidePrompt();
        crosshair.SetActive(false);
        objectiveMarkers.SetActive(false);
        cmBrain.enabled = true;
        tutorialController.ResumeTimelineAndCaptureCamera();
    }

    // 18
    private void RepositionPlayer()
    {
        player.transform.position = zoneAtkPlayerMovePos.position;
        player.transform.rotation = zoneAtkPlayerMovePos.rotation;
        playerAlignedCamera.transform.position = zoneAtkCameraPos.position;
        playerAlignedCamera.transform.rotation = zoneAtkCameraPos.rotation;
    }

    // 19
    private void SpawnZoneAtkWave()
    {
        int slimesCount = 15;
        for (int i = 0; i < slimesCount; ++i)
        {
            AIEnemy slime = zoneDSpawner.SpawnOne(EnemyType.BASIC);
            tutorialEnemiesManager.AddEnemy(slime);
            slime.ignorePath = true;
        }
    }

    // 20
    private void ZoneAttackInfo()
    {
        zoneAttackInfoText.SetActive(true);
    }

    // 21
    private void PlayerZoneTrapLesson()
    {
        Camera.main.transform.position = zoneAtkCameraPos.position;
        Camera.main.transform.rotation = zoneAtkCameraPos.rotation;
        Camera.main.GetComponent<CameraController>().SetCameraXAngle(-75);
        crosshair.SetActive(true);
        objectiveMarkers.SetActive(true);
        infoPromptController.ShowPrompt(infoPrompts[5]);
        player.AddEvilPoints(player.GetMaxEvilLevel() - player.GetEvilLevel());
        tutorialController.NextPlayerState();
        cmBrain.enabled = false;
        zoneTrapWaveEnemyCountMonitor.SetActive(true);
    }

    // 22
    private void ZoneTrapWaveKilled()
    {
        tutorialController.RequestEndTutorial();
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

    private void EvilLimitReached()
    {
        evilMisuseInfoText.gameObject.SetActive(true);
    }
    #endregion
}
