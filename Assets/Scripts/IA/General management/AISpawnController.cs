using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnController : MonoBehaviour {

    #region Fields
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

    private bool waveRunning = false;
    private int nextSpawnIndex;

    [Header("Testing")]
    public bool startWave = false;
    public bool loopWaves = false;
    #endregion

    #region MonoBehaviour Methods
    private void Awake ()
    {
        UnityEngine.Assertions.Assert.IsTrue(enemyTypes.Count == enemyPrefabs.Count, "enemyTypes and enemyPrefabs have different lengths");
        enemies = new Dictionary<EnemyType, AIEnemy>();
        for (int i = 0; i < enemyTypes.Count; ++i)
        {
            enemies.Add(enemyTypes[i], enemyPrefabs[i]);
        }
        VerifyWaveInfos();
    }

    private void Update () {
        if (startWave)
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

            if (elapsedTime > wavesInfo[currentWaveIndex].waveDuration)
            {
                WaveFinished();
            }
        }
	}
    #endregion

    #region Private Methods
    void VerifyWaveInfos()
    {
        Debug.LogError("NOT IMPLEMENTED: AISpawnController::VerifyWaveInfos");
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
        }
    }
    #endregion
}
