﻿using System.Collections.Generic;

[System.Serializable]
public class SpawnInfo
{
    // Used by AISpawnController
    [UnityEngine.Tooltip("Time is expressed in seconds")]
    public float spawnTime;
    // Used by AISpawnController
    public uint spawnerIndex;
    // Used by AISpawner
    [UnityEngine.Tooltip("Duration is expressed in seconds")]
    public float spawnDuration;
    // Used by AISpawner
    public List<EnemyType> enemiesToSpawn;
    [UnityEngine.HideInInspector]
    public float elapsedTime;
}
