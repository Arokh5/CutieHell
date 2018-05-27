using System.Collections.Generic;
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
    public AISpawner zoneDSpawner;
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
    private AIEnemy firstConqueror;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        events = new TutorialEvent[]{
            DropLighting,
            SpawnSlime,
            SlimeAttack,
            SpawnBear,
            BearAttack,
            SpawnConqueror,
            ConquerorAttack,
            Spawn3Slimes,
            HaltEnemies
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
    private void Spawn3Slimes()
    {
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
        tutorialEnemiesManager.AddEnemy(zoneDSpawner.SpawnOne(EnemyType.BASIC));
    }

    // 08
    private void HaltEnemies()
    {
        tutorialEnemiesManager.HaltEnemies();
    }

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
