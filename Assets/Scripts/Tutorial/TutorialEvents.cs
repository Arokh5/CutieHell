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
        SpawnEnemy(tutorialSpawner, EnemyType.BASIC);
        SetEnemyLabelInfo(0);
    }

    private void SpawnEnemy(AISpawner spawner, EnemyType enemyType)
    {
        AIEnemy slime = spawner.SpawnOne(enemyType);
        slime.GetComponent<NavMeshAgent>().enabled = false;
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
