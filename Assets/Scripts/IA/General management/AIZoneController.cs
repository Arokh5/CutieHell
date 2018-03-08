using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZoneController : MonoBehaviour {

    #region Fields
    private uint zoneID;
    public Monument monument;

    [SerializeField]
    private Building currentZoneTarget;

    // List that contains all AIEnemy that were spawned on this ZoneController's area and are still alive
    [SerializeField]
    private List<AIEnemy> aiEnemies;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(monument, "Error: monument not set for AIZoneController in gameObject '" + gameObject.name + "'");
        currentZoneTarget = monument;
    }

    #endregion

    #region Public Methods
    // Called by Monument when it gets repaired
    public void OnMonumentRepaired()
    {
        Debug.LogError("NOT IMPLEMENTED: AIZoneController::OnMonumentRepaired");
    }
    
    // Called by Monument when it gets conquered. The method is meant to open the door
    public void OnMonumentTaken()
    {
        Debug.LogError("NOT IMPLEMENTED: AIZoneController::OnMonumentTaken");
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
    public Building GetTargetBuilding()
    {
        if (currentZoneTarget)
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
        }
    }

    // Called by AIEnemy in its OnDestroy method to remove from the aiEnemies list
    public bool RemoveEnemy(AIEnemy aiEnemy)
    {
        return aiEnemies.Remove(aiEnemy);
    }
    #endregion

    #region Private Methods
    private void OnTargetBuildingChanged()
    {
        foreach (AIEnemy enemy in aiEnemies)
        {
            enemy.SetCurrentTarget(currentZoneTarget);
        }
    }
    #endregion
}
