using System.Collections.Generic;

[System.Serializable]
public class WaveInfo
{
    [UnityEngine.Tooltip("Duration is expressed in seconds")]
    public float waveDuration;
    public List<SpawnInfo> spawnInfos;

    [ShowOnly]
    public float lastEnemySpawnTime;
}
