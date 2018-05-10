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

    [Header("Canon")]
    public Transform canonTargetDecal;
    public Transform canonBallStartPoint;
    public List<CanonBallMotion> canonBallsList = new List<CanonBallMotion>();


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
    // IRepairable
    public override bool CanRepair()
    {
        return !zoneController.monumentTaken && !HasFullHealth();
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
            isActive = true;
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
        if (isActive)
        {
            player = null;
            isActive = false;
            zoneController.OnTrapDeactivated();
        }
    }

    #endregion

    #region Protected Methods
    protected override void BuildingKilled()
    {
        if (isActive)
        {
            isActive = false;
            zoneController.OnTrapDeactivated();
        }

        /* Trap could be killed by a Conqueror after the player got off */
        if (player != null)
        {
            player.StopTrapUse();
            player = null;
        }
    }

    protected override void BuildingRecovered()
    {
        /* Nothing needs to be done here */
    }
    #endregion


}
