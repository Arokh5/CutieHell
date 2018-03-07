using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnController : MonoBehaviour {

    #region Fields
    private float elapsedTime;
    [SerializeField]
    private uint currentWave;
    [SerializeField]
    private List<WaveInfo> wavesInfo;
    [SerializeField]
    private List<AISpawner> aiSpawners;

    public List<EnemyType> enemyTypes;
    public List<AIEnemy> enemyPrefabs;
    public Dictionary<EnemyType, AIEnemy> enemies;

    private bool waveRunning = false;

    [Header("Testing")]
    public bool startWave = false;
    public bool loopWaves = false;
    #endregion

    #region MonoBehaviour Methods
    private void Awake ()
    {
        UnityEngine.Assertions.Assert.IsTrue(enemyTypes.Count == enemyPrefabs.Count, "enemyTypes and enemyPrefabs have different lengths");
        for (int i = 0; i < enemyTypes.Count; ++i)
        {
            enemies.Add(enemyTypes[i], enemyPrefabs[i]);
        }
    }

    private void Update () {
        if (startWave)
        {
            waveRunning = true;
        }

        if (waveRunning)
        {
            // Implement spawning here
            elapsedTime += Time.deltaTime;
        }
	}
    #endregion
}
