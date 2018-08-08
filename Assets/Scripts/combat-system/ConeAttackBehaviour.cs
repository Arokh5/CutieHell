using UnityEngine;

public class ConeAttackBehaviour : PooledParticleSystem
{
    #region Fields
    public LayerMask layerMask;
    public int damage;
    public ConeAttackDetection enemiesDetector;
    public int enemiesToCombo;
    public int evilComboReward;
    public float hurtEnemiesDelay;
    public float timeToDistable;
    private int comboCount;
    private float timer;
    private float timeToReturnToPool;

    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        timer -= Time.deltaTime;
        timeToReturnToPool -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            HurtEnemies();
            timer = 1000.0f;
        }
        if(timeToReturnToPool <= 0.0f)
        {
            ReturnToPool();
        }
    }
    #endregion

    #region Public Methods
    public override void Restart()
    {
        enemiesDetector.attackTargets.Clear();
        timer = hurtEnemiesDelay;
        timeToReturnToPool = timeToDistable;
        comboCount = 0;
    }
    #endregion

    #region Private Methods
    private void HurtEnemies()
    {
        if(enemiesDetector.attackTargets.Count > 0)
        {
            BulletTime.instance.DoSlowmotion(0.01f,0.1f,0.05f);
        }
        foreach (AIEnemy aiEnemy in enemiesDetector.attackTargets)
        {
            aiEnemy.MarkAsTarget(false);
            aiEnemy.SetKnockback(this.transform.position,6.0f);
            aiEnemy.TakeDamage(damage, AttackType.CONE);
            comboCount++;
        }
        enemiesDetector.attackTargets.Clear();
    }
    #endregion
}
