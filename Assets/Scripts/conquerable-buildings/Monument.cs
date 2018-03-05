using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monument : Building {

    #region Public Methods
    // IDamageable
    // If this method is called, it should inform the ZoneController
    public override void FullRepair()
    {
        base.FullRepair();
        zoneController.OnMonumentRepaired();
    }
    #endregion

    #region Protected Methods
    protected override void BuildingKilled()
    {
        zoneController.OnMonumentTaken();
    }
    #endregion
}
