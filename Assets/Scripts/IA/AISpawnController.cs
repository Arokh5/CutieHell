using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnController : MonoBehaviour
{

    #region Fields
    [SerializeField]
    ScenarioController scenario;
    [SerializeField]
    [ShowOnly]
    private float elapsedTime;
    [SerializeField]
    private int currentWaveIndex;
    [SerializeField]
    private List<WaveInfo> wavesInfo;
    [SerializeField]
    private List<AISpawner> aiSpawners;

    public List<EnemyType> enemyTypes;
    public List<AIEnemy> enemyPrefabs;
    public Dictionary<EnemyType, AIEnemy> enemies;

    private bool validWavesInfo = true;
    private bool waveRunning = false;
    private int nextSpawnIndex;

    [Header("Testing")]
    public bool startWave = false;
    public bool loopWaves = false;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (!scenario)
        {
            scenario = GetComponentInParent<ScenarioController>();
            UnityEngine.Assertions.Assert.IsNotNull(scenario, "Error: Scenario not set for AISpawnController in gameObject '" + gameObject.name + "'");
        }

        UnityEngine.Assertions.Assert.IsTrue(enemyTypes.Count == enemyPrefabs.Count, "enemyTypes and enemyPrefabs have different lengths");
        enemies = new Dictionary<EnemyType, AIEnemy>();
        for (int i = 0; i < enemyTypes.Count; ++i)
        {
            enemies.Add(enemyTypes[i], enemyPrefabs[i]);
        }
        validWavesInfo = VerifyWaveInfos();
    }

    private void Update()
    {
        if (startWave && validWavesInfo)
        {
            startWave = false;
            waveRunning = true;
            elapsedTime = 0;
            nextSpawnIndex = 0;
        }

        if (waveRunning)
        {
            if (nextSpawnIndex < wavesInfo[currentWaveIndex].spawnInfos.Count)
            {
                SpawnInfo spawnInfo = wavesInfo[currentWaveIndex].spawnInfos[nextSpawnIndex];
                if (elapsedTime >= spawnInfo.spawnTime)
                {
                    AISpawner spawner = aiSpawners[spawnInfo.spawnerIndex];
                    spawner.Spawn(spawnInfo);
                    ++nextSpawnIndex;
                }
            }
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= wavesInfo[currentWaveIndex].lastEnemySpawnTime)
            {
                scenario.SetLastSpawnIsOver(true);
            }

            if (elapsedTime > wavesInfo[currentWaveIndex].waveDuration)
            {
                WaveFinished();
            }
        }
    }
    #endregion

    #region Private Methods
    bool VerifyWaveInfos()
    {
        for (int w = 0; w < wavesInfo.Count; ++w)
        {
            WaveInfo waveInfo = wavesInfo[w];
            float lastSpawnTime = -1;
            waveInfo.lastEnemySpawnTime = -1;
            for (int s = 0; s < waveInfo.spawnInfos.Count; ++s)
            {
                SpawnInfo spawnInfo = waveInfo.spawnInfos[s];
                if (spawnInfo.spawnTime < lastSpawnTime)
                {
                    Debug.LogError("ERROR: WavesInfo error on AISpawnController. In Wave " + w + ", SpawnInfo " + s + ", has a spawn time that is smaller than the previous SpawnInfo!");
                    return false;
                }
                float lastEnemySpawnTime = spawnInfo.spawnTime + spawnInfo.spawnDuration;
                if (lastEnemySpawnTime > waveInfo.lastEnemySpawnTime)
                {
                    waveInfo.lastEnemySpawnTime = lastEnemySpawnTime;
                }
            }

            if (waveInfo.waveDuration <= waveInfo.lastEnemySpawnTime)
            {
                Debug.LogError("ERROR: WavesInfo error on AISpawnController. In Wave " + w + ", the last enemy would be spawned after the wave duration has finished!");
                return false;
            }
        }
        return true;
    }

    void WaveFinished()
    {
        if (loopWaves)
        {
            elapsedTime = 0;
            nextSpawnIndex = 0;
        }
        else
        {
            waveRunning = false;
            GameManager.instance.OnWaveWon();
        }
    }
    #endregion
}
