using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjection : MonoBehaviour
{

    #region Properties 

    private GameObject summonerTrap;
    private SummonerTrap summonerTrapScript;
    public Transform headTransform;
    public float enemyProjectionSpeed;
    public float enemyProjectionRotationSpeedY;
    public float attractionRadius;
    public float explosionTriggerRadius;
    public float explosionRadius;
    public int explosionDamage;
    public GameObject attractionFX;
    public GameObject explosionVFX;

    private float limitedPlacingDistance;
    private bool enemyProjectionLanded;

    private CameraController camera;

    private List<AIEnemy> attractedEnemies = new List<AIEnemy>();

    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    void Start()
    {
        enemyProjectionLanded = false;
        summonerTrap = transform.parent.gameObject; //Summoner Traps instantiate enemyProjections and set up them as a their child
        summonerTrapScript = summonerTrap.GetComponent<SummonerTrap>();
        camera = summonerTrapScript.camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyProjectionLanded)
        {
            MoveEnemyProjection();
            RotateEnemyProjection();
        }
        else
        {
            EvaluateSelfExplosion();
        }
    }
    #endregion

    #region Public Methods
    public void SetEnemyProjectionLanded(bool landed)
    {
        enemyProjectionLanded = landed;
    }

    public bool GetEnemyProjectionLanded()
    {
        return enemyProjectionLanded;
    }

    public void SetLimitedPlacingDistance(float limitedPlacingDistanceValue)
    {
        limitedPlacingDistance = limitedPlacingDistanceValue;
    }

    public void SetEnemyAttracted(AIEnemy aiEnemyattracted)
    {
        attractedEnemies.Add(aiEnemyattracted);
    }

    public void RemoveEnemyAttracted(AIEnemy deadAIEnemyByOtherWays)
    {
        attractedEnemies.Remove(deadAIEnemyByOtherWays);
    }
    #endregion

    #region Private Methods
    private void MoveEnemyProjection()
    {
        if (InputManager.instance.GetLeftStickUp())
        {
            transform.localPosition += transform.forward * Time.deltaTime * enemyProjectionSpeed;
            if (Vector3.Distance(transform.position, summonerTrap.transform.position) > limitedPlacingDistance)
            {
                transform.localPosition -= transform.forward * Time.deltaTime * enemyProjectionSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickDown())
        {
            transform.localPosition += -transform.forward * Time.deltaTime * enemyProjectionSpeed;
            if (Vector3.Distance(transform.position, summonerTrap.transform.position) > limitedPlacingDistance)
            {
                transform.localPosition += transform.forward * Time.deltaTime * enemyProjectionSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickLeft())
        {
            transform.localPosition += -transform.right * Time.deltaTime * enemyProjectionSpeed;
            if (Vector3.Distance(transform.position, summonerTrap.transform.position) > limitedPlacingDistance)
            {
                transform.localPosition += transform.right * Time.deltaTime * enemyProjectionSpeed;
            }
        }
        if (InputManager.instance.GetLeftStickRight())
        {
            transform.localPosition += transform.right * Time.deltaTime * enemyProjectionSpeed;
            if (Vector3.Distance(transform.position, summonerTrap.transform.position) > limitedPlacingDistance)
            {
                transform.localPosition += -transform.right * Time.deltaTime * enemyProjectionSpeed;
            }
        }

    }

    private void RotateEnemyProjection()
    {
        float y = camera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, y, 0);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision detectada");
        transform.localPosition -= transform.forward * Time.deltaTime;
    }

    private void EvaluateSelfExplosion()
    {
        List<AIEnemy> enemiesWithinTheExplosionTriggerRaddius = summonerTrapScript.ObtainEnemiesAffectedByProjectionExplosion(this.transform, explosionRadius);
        if(enemiesWithinTheExplosionTriggerRaddius != null)
        {
            for (int i = 0; i < enemiesWithinTheExplosionTriggerRaddius.Count; i++)
            {
                AIEnemy triggeringEnemy = enemiesWithinTheExplosionTriggerRaddius[i];
                if (Vector3.Distance(this.transform.position, triggeringEnemy.transform.position) < this.explosionTriggerRadius)
                {
                    ActivateSelfExplosion(null);
                    if (explosionVFX != null) Destroy(Instantiate(explosionVFX, this.transform.position + Vector3.up * 1, this.transform.rotation), 0.9f);
                    break;
                }
            }
        }
               
    }

    private void ActivateSelfExplosion(List<EnemyProjection> enemyProjectionsAffected)
    {
        List<AIEnemy> enemiesDamaged = summonerTrapScript.ObtainEnemiesAffectedByProjectionExplosion(this.transform, explosionRadius);
        if(enemyProjectionsAffected == null)
        {
            enemyProjectionsAffected = summonerTrapScript.ObtainEnemyProjectionsAffectedByProjectionExplosion(this.transform, explosionRadius);
        }

        enemyProjectionsAffected.Remove(this);

        if(enemiesDamaged != null)
        {
            for (int i = 0; i < enemiesDamaged.Count; i++)
            {
                enemiesDamaged[i].TakeDamage(explosionDamage, AttackType.SEDUCTIVE_PROJECTION);
                if (enemiesDamaged[i] == null)
                {
                    RemoveEnemyAttracted(enemiesDamaged[i]);
                }
            }
        }

        summonerTrapScript.DestroyEnemyProjection(this.gameObject);
        if (enemyProjectionsAffected != null)
        {
            for (int i = 0; i < enemyProjectionsAffected.Count; i++)
            {
                if(!this.Equals(enemyProjectionsAffected[i]))
                {
                    enemyProjectionsAffected[i].ActivateSelfExplosion(enemyProjectionsAffected);
                }
            }
        }
        GameObject.Destroy(this);
    }
    #endregion
}