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

    private bool waveRunning = false;

    [Header("Testing")]
    public bool startWave = false;
    public bool loopWaves = false;
    #endregion

    #region MonoBehaviour Methods
    void Update () {
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
