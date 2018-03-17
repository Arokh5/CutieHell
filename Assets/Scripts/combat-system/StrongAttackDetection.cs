using System.Collections.Generic;
using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float strongAttackCadency;
    [SerializeField]
    private int evilCost;
    [SerializeField]
    private int damage;

    private MeshCollider meshCollider;
    private Renderer mRenderer;
    private List<AIEnemy> targetEnemies = new List<AIEnemy>();

    private float cadencyTime;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        cadencyTime = strongAttackCadency;
        meshCollider = GetComponent<MeshCollider>();
        mRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        StrongAttackActivation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(true);
            targetEnemies.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(false);
            targetEnemies.Remove(aIEnemy);
        }
    }
	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Private Methods

    private void StrongAttackActivation()
    {
        if (cadencyTime >= strongAttackCadency && InputManager.instance.GetL2Button() && GameManager.instance.GetPlayer1().GetEvilLevel() >= Mathf.Abs(evilCost))
        {
            if (GameManager.instance.GetPlayer1().state != Player.PlayerStates.TURRET)
            {
                meshCollider.enabled = true;
                mRenderer.enabled = true;

                if (InputManager.instance.GetR2ButtonDown())
                {
                    GameManager.instance.GetPlayer1().SetEvilLevel(evilCost);
                    HurtEnemies();
                }

            }
        }

        if (InputManager.instance.GetL2ButtonUp())
        {
            meshCollider.enabled = false;
            mRenderer.enabled = false;
            /* Untarget all enemies */
            foreach (AIEnemy aiEnemy in targetEnemies)
            {
                aiEnemy.MarkAsTarget(false);
            }
            targetEnemies.Clear();
        }

        cadencyTime += Time.deltaTime;
    }

    private void HurtEnemies()
    {
        foreach (AIEnemy aiEnemy in targetEnemies)
        {
            aiEnemy.TakeDamage(damage, AttackType.STRONG);
        }
        targetEnemies.Clear();

        cadencyTime = 0f;
        meshCollider.enabled = false;
        mRenderer.enabled = false;
    }

	#endregion
}