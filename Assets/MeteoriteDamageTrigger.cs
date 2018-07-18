using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteDamageTrigger : MonoBehaviour {

    public float damage;
    public List<AIEnemy> enemiesInRange;
    private float delayUntilExplosion;

    private void OnEnable()
    {
        delayUntilExplosion = 0.05f;
        enemiesInRange = new List<AIEnemy>();
    }

    private void Update()
    {
        if(delayUntilExplosion < 0.0f)
        {
            HurtEnemies();
        }
        else
        {
            delayUntilExplosion -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            enemiesInRange.Add(other.GetComponent<AIEnemy>());
        }
    }

    private void HurtEnemies()
    {
        foreach (AIEnemy aiEnemy in enemiesInRange)
        {
            aiEnemy.TakeDamage(damage, AttackType.METEORITE);
            aiEnemy.SetKnockback(this.transform.position, 1.0f);
            aiEnemy.SetSlow(0.75f);
        }

    }
}
