using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour {

    #region Fields
    private IDamageable target;
    private float damage;
    public EnemyType enemyType;
    [Tooltip("Speed is expressed in meters per second")]
    public float speed;
    public float maxHitDistance = 1.0f;

    private Vector3 initialPosition;
    private Vector3 fullMotion;
    private float motionDistance;
    private float lifeTime;
    private float elapsedTime;
    private bool attackCheck;
    #endregion

    #region MonoBehaviour Methods
    private void Update () {
	    if (target != null)
        {
            Move();
            if (elapsedTime >= lifeTime && !attackCheck)
            {
                attackCheck = true;
                Attack();
            }
            if (elapsedTime >= lifeTime * 2.0f)
            {
                target = null;
                AttacksPool.instance.ReturnAttackObject(enemyType, gameObject);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        Attack();
    }
    #endregion

    #region Public Methods
    public void Fire(IDamageable target, float damage)
    {
        elapsedTime = 0;
        this.target = target;
        this.damage = damage;
        attackCheck = false;
        transform.LookAt(target.transform);
        initialPosition = transform.position;
        fullMotion = target.transform.position - initialPosition;
        fullMotion.y += 1.5f;
        motionDistance = fullMotion.magnitude;
        lifeTime = motionDistance / speed;
    }
    #endregion

    #region Private Methods
    private void Move()
    {
        float progress = elapsedTime / lifeTime;
        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = initialPosition + fullMotion * progress;
        nextPosition.y += motionDistance * (-0.25f * ((2f * (progress - 0.5f)) * (2f * (progress - 0.5f))) + 0.25f) / 3.25f;
        transform.LookAt(currentPosition + (nextPosition - currentPosition) * 100);
        transform.position = nextPosition;

        elapsedTime += Time.deltaTime;
    }

    private void Attack()
    {
        Vector3 bulletToTarget = target.transform.position - transform.position;
        bulletToTarget.y = 0;
        if (bulletToTarget.magnitude < maxHitDistance)
        {
            target.TakeDamage(damage, AttackType.ENEMY);
            target = null;
            //Instanciar Hit vfx aquí
            AttacksPool.instance.ReturnAttackObject(enemyType, gameObject);
        }
    }
    #endregion
}
