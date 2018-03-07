using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Building, IUsable {

    #region Fields
    public int usageCost;

    private uint trapID;
    private Player player;
    #endregion

    #region Public Methods
    // IUsable
    // Called by Player
    public bool CanUse()
    {
        return currentHealth > 0;
    }

    // Called by Player
    public int GetUsageCost()
    {
        return usageCost;
    }

    // Called by Player. A call to this method should inform the ZoneController
    public bool Activate(Player player)
    {
        if (CanUse())
        {
            this.player = player;
            zoneController.OnTrapActivated(this);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Called by Player. A call to this method should inform the ZoneController
    public void Deactivate()
    {
        player = null;
        zoneController.OnTrapDeactivated();
    }
    #endregion

    #region Protected Methods
    protected override void BuildingKilled()
    {
        player.StopTrapUse();
        Deactivate();
    }
    #endregion
}
