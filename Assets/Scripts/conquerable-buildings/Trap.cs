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
    public Transform rotatingHead;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0 ,0.5f);
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
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
        /* Trap could be killed by a Conqueror after the player got off */
        if (player != null)
        {
            player.StopTrapUse();
        }
    }
    #endregion


}
