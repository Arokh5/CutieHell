using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : MonoBehaviour, IDamageable
{
    #region Fields
    private AIZoneController zoneController;
    private SubZoneType currentSubZone;
    private IDamageable currentTarget;

    [Tooltip("The initial amount of hit points for the conquerable building.")]
    public int baseHealth;

    protected int currentHealth;
    #endregion

    #region MonoBehaviour Methods

    private void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "Error: zoneController is null for AIEnemy in GameObject '" + gameObject.name + "'!");
        UpdateTarget();
    }

    #endregion

    #region Public Methods
    // Called by AISpawner when instantiating an AIEnemy. This method should inform the ZoneController about this AIEnemy's creation
    public void SetZoneController(AIZoneController newZoneController)
    {
        if (!newZoneController)
        {
            return;
        }

        if (zoneController)
        {
            zoneController.RemoveEnemy(this);
        }
        newZoneController.AddEnemy(this);
        zoneController = newZoneController;
    }

    // Called by the ZoneController in case the Monument gets repaired (this will cause all AIEnemy to return to the ZoneController's area)
    // or when a Trap gets deactivated or when the area-type Trap explodes
    public void SetCurrentTarget(IDamageable target)
    {
        currentTarget = target;
    }

    // IDamageable
    // Called by the AIPlayer or an Attack to determine if this AIEnemy should be targetted
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // Called by the AIPlayer or an Attack to damage the AIEnemy
    public void TakeDamage(int damage, AttackType attacktype)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    // Called by the Area-type Trap to retarget the AIEnemy after exploding
    public void UpdateTarget()
    {
        currentTarget = zoneController.GetTargetBuilding();
    }

    #endregion
}
