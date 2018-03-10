using System.Collections.Generic;
using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields

    private const float actionTimeRange = 0.5f;
    private const float strongAttackCadency = 5f;
    private const int evilCost = -10;
    private const int damage = 10;
    private List<AIEnemy> damagedEnemies = new List<AIEnemy>();

    private float actionTime = 0f;
    private float cadencyTime = strongAttackCadency;
    private bool activateAttack = false;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Update()
    {
        StrongAttackActivation();
    }

    private void OnTriggerStay(Collider other)
    {
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