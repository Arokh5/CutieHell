using System.Collections.Generic;
using UnityEngine;

public class MeteoriteDamageTrigger : MonoBehaviour {

    [SerializeField]
    private LayerMask enemiesLayer;
    public float damage;
    public List<AIEnemy> enemiesInRange = new List<AIEnemy>();
    private float delayUntilExplosion;
    private bool triggered;

    private void OnEnable()
    {
        enemiesInRange.Clear();
        delayUntilExplosion = 0.05f;
        triggered = false;
    }

    private void Update()
    {
        if (!triggered)
        {
            if (delayUntilExplosion < 0.0f)
            {
                HurtEnemies();
            }
            else
            {
                delayUntilExplosion -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, enemiesLayer))
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
        triggered = true;
    }
}
