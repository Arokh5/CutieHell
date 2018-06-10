using UnityEngine;

[System.Serializable]
public class SpawnInfo
{
    // Used by AISpawnController
    [Tooltip("Time is expressed in seconds")]
    public float spawnTime;
    // Used by AISpawnController
    public int spawnerIndex;
    // Used by AISpawner
    [Tooltip("Duration is expressed in seconds")]
    public float spawnDuration;
    // Used by AISpawner
    public EnemyType[] enemiesToSpawn;
    [HideInInspector]
    public float elapsedTime;
    [HideInInspector]
    public float nextSpawnTime;
    [HideInInspector]
    public int nextSpawnIndex;
}
