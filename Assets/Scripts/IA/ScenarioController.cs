using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    #region Fields
    private bool lastSpawnIsOver;
    private bool noEnemiesAlive;

    [SerializeField]
    private List<AIZoneController> zoneControllers;
    private int zonesWithEnemiesCount = 0;
    private int zonesConqueredCount = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (zoneControllers == null)
        {
            zoneControllers = new List<AIZoneController>(GetComponentsInChildren<AIZoneController>());
        }
    }
    #endregion

    #region Public Methods
    // Called by a ZoneController when its Monument has been conquered and an AIEnemy request for a Target
    public AIZoneController GetAlternateZone(AIZoneController currentZone)
    {
        Debug.LogError("NOT IMPLEMENTED: ScenarioController::GetAlternateZone");
        return null;
    }

    public void OnWaveTimeOver()
    {
        foreach (AIZoneController zoneController in zoneControllers)
        {
            zoneController.DestroyAllEnemies();
        }
        /* Destroying all enemies of each zoneController will cause calls to OnZoneEmpty. the last call will trigger an GameManager::OnWaveWon */
    }

    public void OnLastEnemySpawned()
    {
        lastSpawnIsOver = true;
    }

    public void OnZoneConquered()
    {
        ++zonesConqueredCount;
        if (zonesConqueredCount == zoneControllers.Count)
        {
            GameManager.instance.OnGameLost();
        }
    }

    public void OnZoneRecovered()
    {
        --zonesConqueredCount;
    }

    public void OnZoneEmpty()
    {
        --zonesWithEnemiesCount;
        if (lastSpawnIsOver && zonesWithEnemiesCount == 0)
        {
            GameManager.instance.OnWaveWon();
        }
    }

    public void OnZoneNotEmpty()
    {
        ++zonesWithEnemiesCount;
    }
    #endregion
}
