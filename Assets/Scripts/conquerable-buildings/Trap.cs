using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Building, IUsable
{

    #region Fields
    [Header("Trap setup")]
    [SerializeField]
    private int trapID;
    public TrapTypes trapType;
    public int usageCost;
    private Player player;

    [Header("Trap testing")]
    public bool activate = false;
    public bool deactivate = false;
    
    [ShowOnly]
    public bool isActive = false;
    #endregion

    public enum TrapTypes { TURRET, SUMMONER }

    #region MonoBehaviour Methods
    private new void Update()
    {
        if (activate)
        {
            isActive = true;
            activate = false;
        }
        else if (deactivate)
        {
            isActive = false;
            deactivate = false;         
        }

        if (isActive)
        {
         
        }

        base.Update();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0 ,0.5f);
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
    #endregion

    #region Public Methods
    // IDamageable
    // If this method is called, it should inform the UIManager
    public override void FullRepair()
    {
        base.FullRepair();
        UIManager.instance.SetTrapConquerRate(zoneController.GetZoneId(), trapID, 0);
    }

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
            activate = true; 
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
        deactivate = true;
        zoneController.OnTrapDeactivated();        
    }

    #endregion

    #region Protected Methods
    protected override void BuildingKilled()
    {
        isActive = false;
        /* Trap could be killed by a Conqueror after the player got off */
        if (player != null)
        {
            player.StopTrapUse();
        }
    }

    protected override void InformUIManager()
    {
        float conquerRate = (baseHealth - currentHealth) / baseHealth;
        UIManager.instance.SetTrapConquerRate(zoneController.GetZoneId(), trapID, conquerRate);
    }
    #endregion


}
