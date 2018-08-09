using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangExplosion : MonoBehaviour
{

    public LayerMask layerMask;
    public List<AIEnemy> attackTargets = new List<AIEnemy>();
    public float damage;

    private float timeToDealDamage = 0.4f;

    private void OnEnable()
    {
        timeToDealDamage = 0.4f;
    }

    private void Update()
    {
        timeToDealDamage -= Time.deltaTime;
        if(timeToDealDamage <= 0.0f)
        {
            HurtEnemies();
            timeToDealDamage = 1000.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            attackTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            attackTargets.Remove(aIEnemy);
        }
    }

    private void HurtEnemies()
    {
        foreach (AIEnemy aiEnemy in attackTargets)
        {
            aiEnemy.SetKnockback(this.transform.position, 1.75f);
            aiEnemy.TakeDamage(damage, AttackType.WEAK);
        }
        attackTargets.Clear();
    }
}
