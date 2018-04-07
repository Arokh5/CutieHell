using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {

    #region Fields
    [SerializeField]
    private AIZoneController zoneController;
    [SerializeField]
    private PathsController pathsController;
    [SerializeField]
    private AISpawnController spawnController;
    [SerializeField]
    private Vector3 spawnerArea;

    [SerializeField]
    private List<SpawnInfo> activeSpawnInfos;

    private List<SpawnInfo> spawnInfosToRemove = new List<SpawnInfo>();
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "ERROR: ZoneController not set for AISpawner in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(pathsController, "ERROR: PathsController not set for AISpawner in gameObject '" + gameObject.name + "'");
        UnityEngine.Assertions.Assert.IsNotNull(spawnController, "ERROR: SpawnController not set for AISpawner in gameObject '" + gameObject.name + "'");
    }

    private void Update()
    {
        foreach (SpawnInfo spawnInfo in activeSpawnInfos)
        {
            spawnInfo.elapsedTime += Time.deltaTime;

            if (spawnInfo.elapsedTime >= spawnInfo.nextSpawnTime)
            {
                EnemyType enemyType = spawnInfo.enemiesToSpawn[spawnInfo.nextSpawnIndex];
                SpawnEnemy(spawnInfo, enemyType);
            }

            if (spawnInfo.nextSpawnIndex >= spawnInfo.enemiesToSpawn.Count)
            {
                spawnInfosToRemove.Add(spawnInfo);
            }
        }

        foreach (SpawnInfo spawnInfo in spawnInfosToRemove)
        {
            spawnInfo.elapsedTime = 0;
            spawnInfo.nextSpawnIndex = 0;
            spawnInfo.nextSpawnTime = 0;
            activeSpawnInfos.Remove(spawnInfo);
        }
        spawnInfosToRemove.Clear();
    }
    #endregion

    #region Public Methods
    // Called by AISpawnController
    public void Spawn(SpawnInfo spawnInfo)
    {
        if (!activeSpawnInfos.Contains(spawnInfo))
        {
            activeSpawnInfos.Add(spawnInfo);
            spawnInfo.elapsedTime = 0;
            spawnInfo.nextSpawnIndex = 0;
            spawnInfo.nextSpawnTime = (spawnInfo.nextSpawnIndex + 1) * spawnInfo.spawnDuration / spawnInfo.enemiesToSpawn.Count;
        }
    }
    #endregion

    #region Private methods
    void SpawnEnemy (SpawnInfo spawnInfo, EnemyType enemyType)
    {
        ++spawnInfo.nextSpawnIndex;
        spawnInfo.nextSpawnTime = spawnInfo.nextSpawnIndex * spawnInfo.spawnDuration / spawnInfo.enemiesToSpawn.Count;

        AIEnemy enemyPrefab = spawnController.enemies[enemyType];
        Vector3 randomPosition = new Vector3(
            Random.Range(0.5f * -spawnerArea.x, 0.5f * spawnerArea.x),
            Random.Range(0.5f * -spawnerArea.y, 0.5f * spawnerArea.y),
            Random.Range(0.5f * -spawnerArea.z, 0.5f * spawnerArea.z)
        );
        randomPosition += transform.position;

        AIEnemy instantiatedEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.LookRotation(transform.forward, transform.up), spawnController.transform);
        instantiatedEnemy.SetZoneController(zoneController);
        instantiatedEnemy.SetPathsController(pathsController);
    }
    #endregion
}
