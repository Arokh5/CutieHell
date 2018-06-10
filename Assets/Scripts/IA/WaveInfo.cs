using UnityEngine;

[System.Serializable]
public class WaveInfo
{
    [Tooltip("Delay counts from the last spawn of the previous wave, unless this is the first wave")]
    public float waveStartDelay;
    public SpawnInfo[] spawnInfos;

    [HideInInspector]
    public float elapsedTime;
    [HideInInspector]
    public int nextSpawnIndex;
    [HideInInspector]
    public float lastEnemySpawnTime;
}
