using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{

    #region Public Methods
    // Called by a ZoneController when its Monument has been conquered and an AIEnemy request for a Target
    public AIZoneController GetAlternateZone(AIZoneController currentZone)
    {
        Debug.LogError("NOT IMPLEMENTED: ScenarioController::GetAlternateZone");
        return null;
    }

    public bool GetLastSpawnIsOver()
    {
        return lastSpawnIsOver;
    }
    public void SetLastSpawnIsOver(bool lastSpawnIsOverValue)
    {
        lastSpawnIsOver = lastSpawnIsOverValue;
    }

    public bool GetNoEnemiesAlive()
    {
        return noEnemiesAlive;
    }
    public void SetNoEnemiesAlive(bool noEnemiesAliveValue)
    {
        noEnemiesAlive = noEnemiesAliveValue;
    }
    #endregion

    #region Private Methods
    private bool lastSpawnIsOver;
    private bool noEnemiesAlive;
    #endregion
}
