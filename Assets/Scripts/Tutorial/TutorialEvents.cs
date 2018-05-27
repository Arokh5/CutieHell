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
    [Header("General")]
    public AISpawner tutorialSpawner;
    [SerializeField]
    private DamageLimiter damageLimiterTutZone;
    public EnemyDescriptionController enemyDescriptionController;
    [SerializeField]
    private EnemyLabelInfo[] enemyLabelInfos;

    [Header("0-DropLighting")]
    public ParticleSystem lightingPrefab;
    public Transform lightingPosition;
    public MonumentIndicator monumentIndicator;

    [Header("1-SpawnSlime")]
    public EnemyType slimeEnemyType;

    private TutorialEvent[] events;
    private TutorialEnemiesManager tutorialEnemiesManager;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        events = new TutorialEvent[]{
            DropLighting,
            SpawnSlime,
            SlimeAttack
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
    }

    public void OnTutorialEnded()
    {
        damageLimiterTutZone.gameObject.SetActive(false);
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
    }

    // 01
    private void SpawnSlime()
    {
        SpawnEnemy(tutorialSpawner, EnemyType.BASIC);
        SetEnemyLabelInfo(0);
        damageLimiterTutZone.normalizedMaxDamage = 0.2f;
    }

    // 02
    private void SlimeAttack()
    {
        tutorialEnemiesManager.ResumeEnemies();
        tutorialEnemiesManager.ClearEnemies();
    }

    private void SpawnEnemy(AISpawner spawner, EnemyType enemyType)
    {
        AIEnemy enemy = spawner.SpawnOne(enemyType);
        tutorialEnemiesManager.AddEnemy(enemy);
        tutorialEnemiesManager.HaltEnemies();
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
