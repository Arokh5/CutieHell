using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour {

    #region Fields
    private Building target;
    private float damage;
    [Tooltip("Speed is expressed in meters per second")]
    public float speed;

    private Vector3 initialPosition;
    private Vector3 fullMotion;
    private Vector3 motionDirection;
    private float motionDistance;
    private float lifeTime;
    private float elapsedTime;
    #endregion

    #region MonoBehaviour Methods
    // Update is called once per frame
    private void Update () {
	    if (target)
        {
            Move();
            if (elapsedTime >= lifeTime)
            {
                Attack();
            }
        }
	}
    #endregion

    #region Public Methods
    public void Fire(Building target, float damage)
    {
        this.target = target;
        this.damage = damage;
        transform.LookAt(target.transform);
        initialPosition = transform.position;
        fullMotion = target.transform.position - initialPosition;
        fullMotion.y = 0;
        motionDistance = fullMotion.magnitude;
        motionDirection = fullMotion / motionDistance;
        lifeTime = motionDistance / speed;
    }
    #endregion

    #region Private Methods
    private void Move()
    {
        float progress = elapsedTime / lifeTime;
        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = initialPosition + fullMotion * progress;
        nextPosition.y += motionDistance * (-0.25f * ((2 * (progress - 0.5f)) * (2 * (progress - 0.5f))) + 0.25f);
        transform.LookAt(currentPosition + (nextPosition - currentPosition) * 100);
        transform.position = nextPosition;

        elapsedTime += Time.deltaTime;
    }

    private void Attack()
    {
        target.TakeDamage(damage, AttackType.ENEMY);
        Destroy(gameObject);
    }
    #endregion
}
