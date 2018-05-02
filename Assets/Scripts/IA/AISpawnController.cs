using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnController : MonoBehaviour
{
    [System.Serializable]
    private class EnemyTypePrefab
    {
        public EnemyType type;
        public AIEnemy prefab;
    }

    #region Fields

    [SerializeField]
    ScenarioController scenario;
    public float firstWaveStartDelay = 0.0f;
    public float nextWavesStartDelay = 10.0f;
    [ShowOnly]
    [SerializeField]
    private float elapsedTime;
    [SerializeField]
    private int currentWaveIndex = -1;
    [SerializeField]
    private int nextSpawnIndex;
    [SerializeField]
    private List<WaveInfo> wavesInfo;
    [SerializeField]
    private List<AISpawner> aiSpawners;

    public Transform pooledEnemies;
    public Transform activeEnemies;

    [SerializeField]
    private EnemyTypePrefab[] enemyPrefabs;
    private Dictionary<EnemyType, ObjectPool<AIEnemy>> enemyPools;

    private bool validWavesInfo = true;
    private bool waveRunning = false;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (!scenario)
        {
            scenario = GetComponentInParent<ScenarioController>();
            UnityEngine.Assertions.Assert.IsNotNull(scenario, "ERROR: Scenario not set for AISpawnController in gameObject '" + gameObject.name + "'");
        }

        UnityEngine.Assertions.Assert.IsNotNull(scenario, "ERROR: enemiesContainer not assigned for AISpawnController in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsTrue(enemyPrefabs.Length > 0, "WARNING: No enemyPrefabs have been assigned for AISpawnController in gameObject '" + gameObject.name + "'");

        UnityEngine.Assertions.Assert.IsNotNull(pooledEnemies, "ERROR: pooledEnemies Transform not assigned for AISpawnController in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(activeEnemies, "ERROR: activeEnemies Transform not assigned for AISpawnController in gameObject '" + gameObject.name + "'");

        enemyPools = new Dictionary<EnemyType, ObjectPool<AIEnemy>>();
        for (int i = 0; i < enemyPrefabs.Length; ++i)
        {
            enemyPools.Add(enemyPrefabs[i].type, new ObjectPool<AIEnemy>(enemyPrefabs[i].prefab, pooledEnemies));
        }
        validWavesInfo = VerifyWaveInfos();
        currentWaveIndex = -1;
    }

    private void Update()
    {
        if (waveRunning)
        {
            WaveInfo currentWaveInfo = wavesInfo[currentWaveIndex];

            if (nextSpawnIndex < currentWaveInfo.spawnInfos.Count)
            {
                SpawnInfo spawnInfo = currentWaveInfo.spawnInfos[nextSpawnIndex];
                if (elapsedTime >= spawnInfo.spawnTime)
                {
                    AISpawner spawner = aiSpawners[spawnInfo.spawnerIndex];
                    spawner.Spawn(spawnInfo);
                    ++nextSpawnIndex;
                }
            }
            elapsedTime += Time.deltaTime;

            /* Update UI */
            UIManager.instance.SetWaveNumberAndProgress(currentWaveIndex + 1, elapsedTime / currentWaveInfo.waveDuration);

            if (elapsedTime >= currentWaveInfo.lastEnemySpawnTime)
            {
                scenario.OnLastEnemySpawned();
            }

            if (elapsedTime > currentWaveInfo.waveDuration)
            {
                WaveFinished();
            }
        }
    }

    #endregion

    #region Public Methods
    public AIEnemy GetEnemy(EnemyType enemyType)
    {
        AIEnemy enemy = enemyPools[enemyType].GetObject(false);
        enemy.spawnController = this;
        return enemy;
    }

    public void ReturnEnemy(AIEnemy enemy)
    {
        enemyPools[enemy.enemyType].ReturnToPool(enemy);
    }

    public bool HasNextWave()
    {
        return validWavesInfo && currentWaveIndex < wavesInfo.Count - 1;
    }

    public bool StartNextWave()
    {
        if (validWavesInfo && currentWaveIndex < wavesInfo.Count - 1)
        {
            ++currentWaveIndex;
            elapsedTime = currentWaveIndex == 0 ? -firstWaveStartDelay : -nextWavesStartDelay;
            nextSpawnIndex = 0;
            waveRunning = true;
            return true;
        }
        else
            return false;
    }

    public bool ForceStartWave(int waveIndex)
    {
        if (validWavesInfo && waveIndex < wavesInfo.Count)
        {
            scenario.OnNewWaveStarted();
            WaveFinished();

            foreach (AISpawner spawner in aiSpawners)
                spawner.ClearSpawnInfos();

            currentWaveIndex = waveIndex;

            waveRunning = true;
            elapsedTime = 0;
            nextSpawnIndex = 0;

            return true;
        }
        return false;
    }

    public void WinCurrentWave()
    {
        if (waveRunning)
        {
            scenario.OnLastEnemySpawned();
            WaveFinished();
            foreach (AISpawner spawner in aiSpawners)
                spawner.ClearSpawnInfos();
        }
    }

    public void RestartCurrentWave()
    {
        ForceStartWave(currentWaveIndex);
    }

    public void StopWave()
    {
        waveRunning = false;
    }

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }
    #endregion

    #region Private Methods
    void WaveFinished()
    {
        waveRunning = false;
        scenario.OnWaveTimeOver();
    }

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
    #endregion
}
