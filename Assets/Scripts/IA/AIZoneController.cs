using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZoneController : MonoBehaviour
{

    #region Fields
    private int zoneID;
    public Monument monument;

    [SerializeField]
    ScenarioController scenario;
    [SerializeField]
    [ShowOnly]
    private Building currentZoneTarget;

    // List that contains all AIEnemy that were spawned on this ZoneController's area and are still alive
    [SerializeField]
    private List<AIEnemy> aiEnemies;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (!scenario)
        {
            scenario = GetComponentInParent<ScenarioController>();
            UnityEngine.Assertions.Assert.IsNotNull(scenario, "Error: Scenario not set for AIZoneController in gameObject '" + gameObject.name + "'");
        }

        UnityEngine.Assertions.Assert.IsNotNull(monument, "Error: monument not set for AIZoneController in gameObject '" + gameObject.name + "'");
        currentZoneTarget = monument;
    }

    #endregion

    #region Public Methods
    // Called by Buildings to obtain zoneId
    public int GetZoneId()
    {
        return zoneID;
    }

    // Called by Monument when it gets repaired
    public void OnMonumentRepaired()
    {
        Debug.LogError("NOT IMPLEMENTED: AIZoneController::OnMonumentRepaired");
        scenario.OnZoneRecovered();
    }

    // Called by Monument when it gets conquered. The method is meant to open the door
    public void OnMonumentTaken()
    {
        Debug.LogWarning("NOT FULLY IMPLEMENTED: AIZoneController::OnMonumentTaken");
        scenario.OnZoneConquered();
    }

    // Called by Trap when it gets activated by Player
    public void OnTrapActivated(Building trap)
    {
        currentZoneTarget = trap;
        OnTargetBuildingChanged();
    }

    // Called by Trap when it gets deactivated by Player
    public void OnTrapDeactivated()
    {
        currentZoneTarget = monument;
        OnTargetBuildingChanged();
    }

    // Called by AIEnemy when it finishes conquering a Building or when the trap it was attacking becomes inactive
    public Building GetTargetBuilding(Transform location)
    {
        Vector3 buildingToLocation = location.position - currentZoneTarget.transform.position;
        buildingToLocation.y = 0;
        if (buildingToLocation.sqrMagnitude < currentZoneTarget.attractionRadius * currentZoneTarget.attractionRadius)
        {
            return currentZoneTarget;
        }
        else
        {
            return monument;
        }
    }

    // Called by AIEnemy during its configuration to add it to the aiEnemies list
    public void AddEnemy(AIEnemy aiEnemy)
    {
        if (!aiEnemies.Contains(aiEnemy))
        {
            aiEnemies.Add(aiEnemy);
            /* If we just added a first enemy*/
            if (aiEnemies.Count == 1)
            {
                scenario.OnZoneNotEmpty();
            }
        }
    }

    // Called by AIEnemy in its OnDestroy method to remove from the aiEnemies list
    public bool RemoveEnemy(AIEnemy aiEnemy)
    {
        bool removed;
        removed = aiEnemies.Remove(aiEnemy);
        if (aiEnemies.Count == 0)
        {
            scenario.OnZoneEmpty();
        }
        return removed;
    }

    public void DestroyAllEnemies()
    {
        foreach (AIEnemy aiEnemy in aiEnemies)
        {
            Destroy(aiEnemy.gameObject);
        }
        aiEnemies.Clear();
        scenario.OnZoneEmpty();
    }
    #endregion

    #region Private Methods
    private void OnTargetBuildingChanged()
    {
        if (currentZoneTarget.attractionRadius != 0)
        {
            foreach (AIEnemy enemy in aiEnemies)
            {
                if (currentZoneTarget.attractionRadius < 0)
                {
                    enemy.SetCurrentTarget(currentZoneTarget);
                }
                else
                {
                    /* Consider distance in XZ-Plane */
                    Vector3 buildingToEnemy = enemy.transform.position - currentZoneTarget.transform.position;
                    buildingToEnemy.y = 0;
                    if (buildingToEnemy.sqrMagnitude < currentZoneTarget.attractionRadius * currentZoneTarget.attractionRadius)
                    {
                        enemy.SetCurrentTarget(currentZoneTarget);
                    }
                }
            }
        }
    }
    #endregion
}
