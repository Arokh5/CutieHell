using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ActivateMineExplosion : PooledParticleSystem
{
    [SerializeField]
    private MineTargets mineTargets;
    [SerializeField]
    private ParticleSystem explosionVFX;
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
                aiEnemy.SetSlow(8.0f);
            }
            ParticlesManager.instance.LaunchParticleSystem(explosionVFX, this.transform.position, explosionVFX.transform.rotation);
            CameraShaker.Instance.ShakeOnce(0.5f, 4.5f, 0.1f, 0.7f);
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
