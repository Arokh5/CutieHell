using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMineExplosion : PooledParticleSystem
{
    [SerializeField]
    private MineTargets mineTargets;

    [SerializeField]
    private SphereCollider sphereCollider;
    private float timeOnActivate;

    //Parameters
    [SerializeField]
    private float timeToActivate;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float knockBack;

    private void Update()
    {
        if(timeOnActivate > timeToActivate)
        {
            sphereCollider.enabled = true;
        }
        timeOnActivate += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            if(mineTargets.currentMineTargets.Count == 0)
            {
                
            }
            foreach (AIEnemy aiEnemy in mineTargets.currentMineTargets)
            {
                aiEnemy.TakeDamage(damage, AttackType.MINE);
                aiEnemy.SetKnockback(this.transform.position, knockBack);
            }
            mineTargets.currentMineTargets.Clear();
            GameManager.instance.GetPlayer1().RemoveMine(this);
            ReturnToPool();
        }
    }

    public void DestroyMine()
    {
        ReturnToPool();
    }

    public override void Restart()
    {
        sphereCollider.enabled = false;
        timeOnActivate = 0.0f;
        enabled = true;
    }
}
