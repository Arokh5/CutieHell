using UnityEngine;
using UnityEngine.AI;

public class TutorialEvents: MonoBehaviour
{
    private delegate void TutorialEvent();

    #region Fields
    [Header("0-DropLighting")]
    public ParticleSystem lightingPrefab;
    public Transform lightingPosition;
    public MonumentIndicator monumentIndicator;

    [Header("1-SpawnSlime")]
    public AISpawner spawnerD;
    public EnemyType slimeEnemyType;

    private TutorialEvent[] events;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        events = new TutorialEvent[]{
            DropLighting,
            SpawnSlime
        };
    }
    #endregion

    #region Public Methods
    public void LaunchEvent(int eventIndex)
    {
        if (eventIndex >= 0 && eventIndex < events.Length)
            events[eventIndex]();
    }
    #endregion

    #region Private Methods
    // 0
    private void DropLighting()
    {
        ParticlesManager.instance.LaunchParticleSystem(lightingPrefab, lightingPosition.position, lightingPrefab.transform.rotation);
        monumentIndicator.RequestOpen();
    }

    //1
    private void SpawnSlime()
    {
        SpawnEnemy(spawnerD, EnemyType.BASIC);
    }

    private void SpawnEnemy(AISpawner spawner, EnemyType enemyType)
    {
        AIEnemy slime = spawner.SpawnOne(enemyType);
        slime.GetComponent<NavMeshAgent>().enabled = false;
    }
    #endregion
}
