using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {

    #region Fields
    [SerializeField]
    private AIZoneController zonecontroller;

    [SerializeField]
    private List<SpawnInfo> activeSpawnInfos;
    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        
    }
    #endregion

    #region Public Methods
    // Called by AISpawnController
    public void Spawn(SpawnInfo spawnInfo)
    {
        if (!activeSpawnInfos.Contains(spawnInfo))
        {
            activeSpawnInfos.Add(spawnInfo);
        }
    }
    #endregion
}
