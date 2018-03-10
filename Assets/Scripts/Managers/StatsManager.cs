using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {

    #region Fields
    public static StatsManager instance;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Public Methods
    // Called by AIEnemy upon dying
    public void RegisterKill(EnemyType enemyType, AttackType attackType)
    {
        Debug.LogError("NOT IMPLEMENTED: StatsManager::RegisterKill");
    }

    // Called by Player when gaining EP (Evil Points)
    public void RegisterEPGained(int epGained)
    {
        Debug.LogError("NOT IMPLEMENTED: StatsManager::RegisterEPGained");
    }

    // Called by Player when gaining EP but losing it due to having reached the max EP (Evil Points)
    public void RegisterEPLost(int epLost)
    {
        Debug.LogError("NOT IMPLEMENTED: StatsManager::RegisterEPLost");
    }

    // Called by Player when using EP (Evil Points)
    public void RegisterEPUsed(int epUsed)
    {
        Debug.LogError("NOT IMPLEMENTED: StatsManager::RegisterEPUsed");
    }
    #endregion
}
