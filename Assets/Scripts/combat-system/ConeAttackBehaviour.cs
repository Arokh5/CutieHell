using UnityEngine;

public class ConeAttackBehaviour : PooledParticleSystem
{
    #region Fields
    public LayerMask layerMask;
    public int damage;
    public ConeAttackDetection enemiesDetector;
    public int enemiesToCombo;
    public float hurtEnemiesDelay;
    public float timeToDisable;

    public bool hitOverTime;
    [Tooltip("The time (in seconds) over which the enemies are hit")]
    public float hitSpreadDuration;

    private int comboCount;
    private float timer;
    private float timeToReturnToPool;

    private float hitWaitTime;
    private float timeToNextHit;
    private bool hittingOverTime;

    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        timer -= Time.deltaTime;
        timeToReturnToPool -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            LaunchBulletTime();
            if (hitOverTime)
            {
                SetUpHitOverTime();
            }
            else
            {
                HitAll();
            }
            timer = 1000.0f;
        }

        if (hittingOverTime)
        {
            HitOverTime();
        }

        if(timeToReturnToPool <= 0.0f)
        {
            HitAll();
            ReturnToPool();
        }
    }

    private void OnValidate()
    {
        if (hitSpreadDuration < 0.0f)
            hitSpreadDuration = 0.0f;

        if (hitSpreadDuration > timeToDisable - hurtEnemiesDelay)
        {
            Debug.LogWarning("WARNING: (ConeAttackBehaviour): Can't set the HitSpreadDuration to a value larger than the 'Time To Disable' minus the 'Hurt Enemies Delay'!");
            hitSpreadDuration = timeToDisable - hurtEnemiesDelay;
        }
    }
    #endregion

    #region Public Methods
    public override void Restart()
    {
        enemiesDetector.attackTargets.Clear();
        timer = hurtEnemiesDelay;
        timeToReturnToPool = timeToDisable;
        comboCount = 0;

        hitWaitTime = 0.0f;
        hittingOverTime = false;
    }
    #endregion

    #region Private Methods
    private void LaunchBulletTime()
    {
        if(enemiesDetector.attackTargets.Count > 0)
        {
            BulletTime.instance.DoSlowmotion(0.01f,0.1f,0.05f);
        }
    }

    private void SetUpHitOverTime()
    {
        if (enemiesDetector.attackTargets.Count > 0)
        {
            hitWaitTime = hitSpreadDuration / enemiesDetector.attackTargets.Count;
            timeToNextHit = 0.0f;
            hittingOverTime = true;
        }
    }

    private void HitOverTime()
    {
        timeToNextHit -= Time.deltaTime;
        if (timeToNextHit <= 0.0f)
        {
            timeToNextHit += hitWaitTime;
            AIEnemy aiEnemy = enemiesDetector.attackTargets[0];
            enemiesDetector.attackTargets.RemoveAt(0);
            HitOne(aiEnemy);
        }
        if (enemiesDetector.attackTargets.Count == 0)
            hittingOverTime = false;
    }

    private void HitAll()
    {
        foreach (AIEnemy aiEnemy in enemiesDetector.attackTargets)
        {
            HitOne(aiEnemy);
        }
        enemiesDetector.attackTargets.Clear();
    }

    private void HitOne(AIEnemy aiEnemy)
    {
        aiEnemy.MarkAsTarget(false);
        aiEnemy.SetKnockback(transform.position, 6.0f);
        aiEnemy.TakeDamage(damage, AttackType.CONE);
        comboCount++;
    }
    #endregion
}
