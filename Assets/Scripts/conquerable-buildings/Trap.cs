using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Building, IUsable {

    #region Fields
    [Header("Trap setup")]
    public int usageCost;

    private uint trapID;
    private Player player;

    [Header("Trap testing")]
    public bool activate = false;
    public bool deactivate = false;
    [ShowOnly]
    public bool isActive = false;
    #endregion

    #region MonoBehaviour Methods
    private new void Update()
    {
        if (activate)
        {
            activate = false;
            isActive = true;
            Activate(player);
        }
        if (deactivate)
        {
            deactivate = false;
            isActive = false;
            Deactivate();
        }
        base.Update();
    }
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
        isActive = false;
        if (player != null)
        {
            player.StopTrapUse();
        }
        else
        {
            Debug.LogError("Trap is still in testing mode using a null Player for activation!");
        }
        Deactivate();
    }
    #endregion
}
