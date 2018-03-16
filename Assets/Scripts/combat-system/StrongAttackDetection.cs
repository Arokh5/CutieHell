using System.Collections.Generic;
using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float actionTimeRange;
    [SerializeField]
    private float strongAttackCadency;
    [SerializeField]
    private int evilCost;
    [SerializeField]
    private int damage;

    private List<AIEnemy> damagedEnemies = new List<AIEnemy>();
    private float actionTime = 0f;
    private float cadencyTime;
    private bool activateAttack = false;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        cadencyTime = strongAttackCadency;
    }

    private void Update()
    {
        StrongAttackActivation();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<AIEnemy>().MarkAsTarget(true);
        }

        if (activateAttack)
        {
            if (other.gameObject.layer == 8)
            {
                HurtEnemies(other.GetComponent<AIEnemy>());
            }
        }
    }

	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Private Methods

    private void StrongAttackActivation()
    {
        if (InputManager.instance.GetL2Button() && !activateAttack && GameManager.instance.GetPlayer1().GetEvilLevel() >= Mathf.Abs(evilCost) && cadencyTime >= strongAttackCadency)
        {
            if (GameManager.instance.GetPlayer1().state != Player.PlayerStates.TURRET)
            {
                if (actionTime < actionTimeRange)
                {
                    GetComponent<MeshCollider>().enabled = true;
                    GetComponent<Renderer>().enabled = true;

                    if (InputManager.instance.GetR2ButtonDown())
                    {
                        activateAttack = true;
                        GameManager.instance.GetPlayer1().SetEvilLevel(evilCost);
                    }
                }
            }
        }

        if (InputManager.instance.GetL2ButtonUp())
        {
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }

        if (activateAttack)
        {
            actionTime += Time.deltaTime;
        }

        if (actionTime >= actionTimeRange)
        {
            activateAttack = false;
            actionTime = 0f;
            cadencyTime = 0f;
            damagedEnemies.Clear();
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }

        cadencyTime += Time.deltaTime;
    }

    private void HurtEnemies(AIEnemy enemy)
    {
        bool repeated = false;

        foreach (AIEnemy damagedEnemy in damagedEnemies)
        {
            if (enemy == damagedEnemy)
            {
                repeated = true;
                break;
            }
        }

        if (!repeated)
        {
            damagedEnemies.Add(enemy);
            enemy.TakeDamage(damage, AttackType.STRONG);
        }
    }

	#endregion
}