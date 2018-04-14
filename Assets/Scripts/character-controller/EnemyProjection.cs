using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjection : MonoBehaviour
{

    #region Properties 

    private GameObject summonerTrap;
    private Trap summonerTrapScript;

    public Transform headTransform;
    public float enemyProjectionSpeed;
    public float enemyProjectionRotationSpeedY;
    public float attractionRadius;
    public float explosionTriggerRadius;
    public float explosionRadius;

    private float limitedPlacingDistance;
    private bool enemyProjectionLanded;

    private List<AIEnemy> attractedEnemies = new List<AIEnemy>();

    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    void Start()
    {
        enemyProjectionLanded = false;
        summonerTrap = transform.parent.gameObject;
        summonerTrapScript = summonerTrap.GetComponent<Trap>();

        Debug.Log("Serializar");
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
        float y = transform.rotation.eulerAngles.y;

        if (InputManager.instance.GetRightStickLeft() || InputManager.instance.GetRightStickRight())
            y += enemyProjectionRotationSpeedY * InputManager.instance.GetRightStickLeftValue();

        transform.rotation = Quaternion.Euler(0, y, 0);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision detectada");
        transform.localPosition -= transform.forward * Time.deltaTime;
    }

    private void EvaluateSelfExplosion()
    {
        for (int i = 0; i < attractedEnemies.Count; i++)
        {
            AIEnemy attractedEnemy = attractedEnemies[i];
            if (Vector3.Distance(this.transform.position, attractedEnemy.transform.position) < this.explosionTriggerRadius)
            {
                ActivateSelfExplosion();
                break;
            }
        }       
    }

    private void ActivateSelfExplosion()
    {
        List<AIEnemy> enemiesToRemove = new List<AIEnemy>();
        for (int i = 0; i < attractedEnemies.Count; i++)
        {
            AIEnemy attractedEnemy = attractedEnemies[i];
            if (Vector3.Distance(this.transform.position, attractedEnemy.transform.position) < this.explosionRadius)
            {
                attractedEnemy.TakeDamage(attractedEnemy.baseHealth, AttackType.SEDUCTIVE_PROJECTION);
                enemiesToRemove.Add(attractedEnemy);
            }
        }
        
        for (int i = 0; i < enemiesToRemove.Count; i++)
        {
            RemoveEnemyAttracted(enemiesToRemove[i]);
        }
        summonerTrapScript.DestroyEnemyProjection(this.gameObject);
        GameObject.Destroy(this);
    }
    #endregion
}