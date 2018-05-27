using UnityEngine;
using UnityEngine.AI;

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

    [Header("General")]
    public AISpawner tutorialSpawner;
    public AISpawner tutorialSpawner2;
    [SerializeField]
    private DamageLimiter damageLimiterTutZone;
    public EnemyDescriptionController enemyDescriptionController;
    [SerializeField]
    private EnemyLabelInfo[] enemyLabelInfos;

    [Header("0-DropLighting")]
    public ParticleSystem lightingPrefab;
    public Transform lightingPosition;
    public MonumentIndicator monumentIndicator;

    private TutorialEvent[] events;
    private TutorialEnemiesManager tutorialEnemiesManager;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        events = new TutorialEvent[]{
            DropLighting,
            SpawnSlime,
            SlimeAttack,
            SpawnBear,
            BearAttack
        };
    }
    #endregion

    #region Public Methods
    public void SetTutorialEnemiesManager(TutorialEnemiesManager tutorialEnemiesManager)
    {
        this.tutorialEnemiesManager = tutorialEnemiesManager;
    }

    public void OnTutorialStarted()
    {
        damageLimiterTutZone.gameObject.SetActive(true);

        foreach (GameObject go in bannersAndMarkers)
            go.SetActive(false);

        tutObjectiveIcon.SetActive(true);
        tutObjectiveMarker.SetActive(false);
    }

    public void OnTutorialEnded()
    {
        damageLimiterTutZone.gameObject.SetActive(false);

        foreach (GameObject go in bannersAndMarkers)
            go.SetActive(true);

        tutObjectiveIcon.SetActive(false);
        tutObjectiveMarker.SetActive(false);
    }

    public void LaunchEvent(int eventIndex)
    {
        if (eventIndex >= 0 && eventIndex < events.Length)
            events[eventIndex]();
        else
            Debug.LogWarning("WARNING: eventIndex parameter out of range in TutorialEvents::LaunchEvent in gameObject '" + gameObject.name + "'!");
    }
    #endregion

    #region Private Methods
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
        SpawnEnemy(tutorialSpawner, EnemyType.BASIC);
        tutorialEnemiesManager.HaltEnemies();
        SetEnemyLabelInfo(0);
        damageLimiterTutZone.normalizedMaxDamage = 0.2f;
    }

    // 02
    private void SlimeAttack()
    {
        tutorialEnemiesManager.ResumeEnemies();
        tutorialEnemiesManager.ClearEnemies();
    }

    // 03
    private void SpawnBear()
    {
        SpawnEnemy(tutorialSpawner2, EnemyType.RANGE);
        tutorialEnemiesManager.HaltEnemies();
        SetEnemyLabelInfo(1);
        damageLimiterTutZone.normalizedMaxDamage = 0.5f;
    }

    // 04
    private void BearAttack()
    {
        tutorialEnemiesManager.ResumeEnemies();
        tutorialEnemiesManager.ClearEnemies();
    }

    private void SpawnEnemy(AISpawner spawner, EnemyType enemyType)
    {
        AIEnemy enemy = spawner.SpawnOne(enemyType);
        tutorialEnemiesManager.AddEnemy(enemy);
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
