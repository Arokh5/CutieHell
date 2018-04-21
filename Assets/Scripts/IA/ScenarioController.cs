using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    #region Fields
    private bool lastSpawnIsOver;
    private bool noEnemiesAlive;

    [SerializeField]
    private AISpawnController spawnController;
    [SerializeField]
    private List<AIZoneController> zoneControllers;
    private int zonesWithEnemiesCount = 0;
    private int zonesConqueredCount = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (spawnController == null)
        {
            spawnController = GetComponentInChildren<AISpawnController>();
        }
        UnityEngine.Assertions.Assert.IsNotNull(spawnController, "ERROR: No AISpawnController found by the ScenarioController in gameObject '" + gameObject.name + "'");

        if (zoneControllers == null || zoneControllers.Count == 0)
        {
            zoneControllers = new List<AIZoneController>(GetComponentsInChildren<AIZoneController>());
        }
        UnityEngine.Assertions.Assert.IsTrue(zoneControllers.Count > 0, "ERROR: No AIZoneControllers found by the ScenarioController in gameObject '" + gameObject.name + "'");
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
            OnZoneEmpty();
        }
    }

    public void OnNewWaveStarted()
    {
        lastSpawnIsOver = false;
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
            spawnController.StopWave();
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
            spawnController.StopWave();
            GameManager.instance.OnWaveWon();
        }
    }

    public void OnZoneNotEmpty()
    {
        ++zonesWithEnemiesCount;
    }
    #endregion
}
