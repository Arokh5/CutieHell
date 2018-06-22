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
    public float usageCost;
    private Player player;
    public Transform rotatingHead;
    [SerializeField]
    private GameObject trapIndicator;
    [SerializeField]
    private PercentageCounter trapPercentageCounter;

    [Header("Trap testing")]
    public bool activate = false;
    public bool deactivate = false;

    [Header("Canon")]
    public GameObject canonTargetDecal;
    public Transform canonBallStartPoint;
    public ParticleSystem canonShootingSmokeVFX;
    public List<CanonBallMotion> canonBallsList = new List<CanonBallMotion>();
    private CanonBallInfo canonBallInfo;
    [SerializeField]
    private ImageExchanger ammunitionUIExchanger;

    private WaitForSeconds canonWaitForSeconds = new WaitForSeconds(0.5f);

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
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attractionRadius);

        if (canonBallsList.Count > 0)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(canonBallsList[0].transform.position, canonBallInfo.canonBallExplosionRange);
        }
    }
    #endregion

    #region Public Methods
    public override void TakeDamage(float damage, AttackType attacktype)
    {
        base.TakeDamage(damage, attacktype);
        trapPercentageCounter.UpdatePercentage(((baseHealth - currentHealth) / baseHealth) * 100);
    }

    // IUsable
    // Called by Player
    public bool CanUse()
    {
        return currentHealth > 0;
    }

    // Called by Player
    public float GetUsageCost()
    {
        return usageCost;
    }

    //Called by CanonTrapEnterAction
    public GameObject GetCurrentTrapIndicator()
    {
        return trapIndicator;
    }

    public ImageExchanger GetCanonAmmunitionImageExchanger()
    {
        return ammunitionUIExchanger;
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
    public override void BuildingKilled()
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

    public override void BuildingConverted()
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
            evaluatedCanonBall.canonBallFiringTime += Time.deltaTime;
            motionProgress = evaluatedCanonBall.canonBallFiringTime / evaluatedCanonBall.canonBallShootingDuration;

            if (!evaluatedCanonBall.canonBallRenderer.enabled && motionProgress >= evaluatedCanonBall.canonBallVisibleFromProgression)
            {
                evaluatedCanonBall.canonBallRenderer.enabled = true;
                if (!canonShootingSmokeVFX.gameObject.activeSelf)
                {
                    canonShootingSmokeVFX.gameObject.SetActive(true);
                }
                else
                {
                    canonShootingSmokeVFX.Play();
                }
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
                if (canonBallInfo.canonBallExplosionVFX != null) Destroy(Instantiate(canonBallInfo.canonBallExplosionVFX, evaluatedCanonBall.transform.position, this.transform.rotation), 3f);

                StartCoroutine(CanonRangedDamageCoroutine(evaluatedCanonBall, canonBallInfo.canonBallExplosionRange));           
            }            
        }
    }

    IEnumerator CanonRangedDamageCoroutine(CanonBallMotion canonBall, float explosionRange)
    {
        canonBall.gameObject.SetActive(false);
        canonBallsList.Remove(canonBall);

        yield return canonWaitForSeconds;

        List<AIEnemy> affectedEnemies = ObtainEnemiesAffectedByTrapRangedDamage(canonBall.transform, explosionRange);
        for (int j = 0; j < affectedEnemies.Count; j++)
        {
            affectedEnemies[j].TakeDamage(canonBallInfo.canonBallExplosionRange, AttackType.TRAP_AREA);
        }

        Destroy(canonBall.gameObject);
    }
    #endregion
}
