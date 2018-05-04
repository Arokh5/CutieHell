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

    private void Start()
    {
        for (int i = 0; i < zoneControllers.Count; ++i)
        {
            AIZoneController zoneController = zoneControllers[i];
            zoneController.zoneID = i;
        }
    }
    #endregion

    #region Public Methods

    // Called by a ZoneController when its Monument has been conquered and an AIEnemy request for a Target
    public Monument GetAlternateTarget(AIZoneController currentZone)
    {
        for (int i = currentZone.zoneID - 1; i >= 0; --i)
        {
            AIZoneController alternatezonecontroller = zoneControllers[i];
            if (!alternatezonecontroller.monumentTaken)
            {
                return alternatezonecontroller.monument;
            }
        }
        UnityEngine.Assertions.Assert.IsTrue(false, "ERROR: A line in ScenarioController::GetAlternateTarget that shouldn't be reached has been reached!");
        return null;
    }

    public void OnWaveTimeOver()
    {
        bool onZoneEmptyCalled = false;
        foreach (AIZoneController zoneController in zoneControllers)
        {
            if (zoneController.HasEnemies())
            {
                zoneController.DestroyAllEnemies();
                OnZoneEmpty();
                onZoneEmptyCalled = true;
            }
        }

        if (!onZoneEmptyCalled)
            CheckWaveWon();
    }

    public void OnNewWaveStarted()
    {
        lastSpawnIsOver = false;
    }

    public void OnLastEnemySpawned()
    {
        lastSpawnIsOver = true;
    }

    public void OnFinalZoneConquered()
    {
        spawnController.StopWave();
        GameManager.instance.OnGameLost();
    }

    public void OnZoneEmpty()
    {
        --zonesWithEnemiesCount;
        CheckWaveWon();
    }

    public void OnZoneNotEmpty()
    {
        ++zonesWithEnemiesCount;
    }
    #endregion

    #region Private Methods

    private void CheckWaveWon()
    {
        if (lastSpawnIsOver && zonesWithEnemiesCount == 0)
        {
            spawnController.StopWave();
            GameManager.instance.OnWaveWon();
        }
    }

    #endregion
}
