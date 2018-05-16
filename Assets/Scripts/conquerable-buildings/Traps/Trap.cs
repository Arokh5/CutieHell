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
    [SerializeField]
    private CurrentTrapIndicator trapIndicator;

    [Header("Trap testing")]
    public bool activate = false;
    public bool deactivate = false;

    [Header("Canon")]
    public Transform canonTargetDecal;
    public Transform canonBallStartPoint;
    public List<CanonBallMotion> canonBallsList = new List<CanonBallMotion>();
    private CanonBallInfo canonBallInfo;


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

        if(canonBallsList.Count > 0)
        {          
            UpdateCanonBallsMotion();
        }
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

    public override void TakeDamage(float damage, AttackType attacktype)
    {
        base.TakeDamage(damage, attacktype);
        trapIndicator.SetFill((baseHealth - currentHealth) / baseHealth);
    }

    public override void FullRepair()
    {
            base.FullRepair();
            trapIndicator.SetFill(0);
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

    // Called by TrapEnterAction
    public CurrentTrapIndicator GetCurrentTrapIndicator()
    {
        return trapIndicator;
    }

    public List<AIEnemy> ObtainEnemiesAffectedByTrapRangedDamage(Transform emissionTransform, float aoeRange)
    {
        List<AIEnemy> affectedEnemies = zoneController.GetEnemiesWithinRange(emissionTransform, aoeRange);

        return affectedEnemies;
    }

    public void SetCanonBallInfo(CanonBallInfo canonBallNewInfo)
    {
        canonBallInfo = canonBallNewInfo;
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

    #region Private Methods
    private void UpdateCanonBallsMotion()
    {
        float motionProgress = 0;
        CanonBallMotion evaluatedCanonBall;
        Vector3 nextPosition;

        for (int i = 0; i < canonBallsList.Count; i++)
        {
            evaluatedCanonBall = canonBallsList[i];
            evaluatedCanonBall.canonBallElapsedTime += Time.deltaTime;
            motionProgress = evaluatedCanonBall.canonBallElapsedTime / evaluatedCanonBall.canonBallShootingDuration;

            if (!evaluatedCanonBall.canonBall.gameObject.activeSelf && motionProgress >= evaluatedCanonBall.canonBallVisibleFromProgression)
            {
                evaluatedCanonBall.canonBall.gameObject.SetActive(true);
            }

            nextPosition = canonBallStartPoint.position - evaluatedCanonBall.canonBallShotingDistance * motionProgress;

            if (motionProgress <= 0.5f) //Ascendent Halfway
            {
                nextPosition.y += evaluatedCanonBall.canonBallShotingDistance.magnitude * (motionProgress * canonBallInfo.canonBallParabolaHeight);
            }
            else // Descendent Halfway
            {
                nextPosition.y += evaluatedCanonBall.canonBallShotingDistance.magnitude * ((1 - motionProgress) * canonBallInfo.canonBallParabolaHeight);
            }

            evaluatedCanonBall.transform.position = nextPosition;

            if (motionProgress >= 1 || evaluatedCanonBall.GetHasToExplode())
            {
                List<AIEnemy> affectedEnemies = ObtainEnemiesAffectedByTrapRangedDamage(evaluatedCanonBall.transform, canonBallInfo.canonBallExplosionRange);

                for (int j = 0; j < affectedEnemies.Count; j++)
                {
                    affectedEnemies[j].TakeDamage(canonBallInfo.canonBallExplosionRange, AttackType.TRAP_AREA);
                }

                canonBallsList.Remove(evaluatedCanonBall);
                Destroy(evaluatedCanonBall.canonBall.gameObject);
            }            
        }
    }
    #endregion
}
