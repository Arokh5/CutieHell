using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monument : Building {

    #region Public Methods
    // IDamageable
    // If this method is called, it should inform the ZoneController and UIManager
    public override void FullRepair()
    {
        base.FullRepair();
        UIManager.instance.SetMonumentConquerRate(zoneController.GetZoneId(), 0);
        zoneController.OnMonumentRepaired();
    }
    #endregion

    #region Protected Methods
    protected override void BuildingKilled()
    {
        zoneController.OnMonumentTaken();
    }

    protected override void InformUIManager()
    {
        float conquerRate = (baseHealth - currentHealth) / baseHealth;
        UIManager.instance.SetMonumentConquerRate(zoneController.GetZoneId(), conquerRate);
    }
    #endregion
}
