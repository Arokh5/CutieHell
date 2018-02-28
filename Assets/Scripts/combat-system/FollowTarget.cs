using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    #region Fields

    private const float attackSpeed = 15f;
    private const float lifeTime = 10f;
    private Transform enemy = null;
    private float time = 0;

    #endregion

    #region Properties

    public void SetEnemy(Transform enemy)
    {
        this.enemy = enemy;
    }

    #endregion

    #region MonoBehaviour Methods

    private void Update()
    {
        SetOrbDirection();
        DestroyOrb();
    }

	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Private Methods
	
    private void SetOrbDirection()
    {
        if (enemy != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemy.position, attackSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * attackSpeed * Time.deltaTime);
        }
    }

    private void DestroyOrb()
    {
        time += Time.deltaTime;

        if (time >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

	#endregion
}