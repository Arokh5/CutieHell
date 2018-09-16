using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : PooledParticleSystem
{

    #region Fields
    public LayerMask layerMask;
    public List<AIEnemy> attackTargets = new List<AIEnemy>();
    public float slowRange, killRange;
    public SphereCollider sphereCollider;
    public float duration;
    private float sqrKillRange;
    private float timer;
    #endregion

    #region MonoBehaviour Methods
    void OnEnable()
    {
        timer = 0.0f;
        sphereCollider.radius = slowRange;
        attackTargets.Clear();
        sqrKillRange = killRange * killRange;
        GameManager.instance.GetPlayer1().SetIsBlackHoleOn(true);
    }

    void Update()
    {
        AttackLogic();
        timer += Time.deltaTime;
        if(timer >= duration)
        {
            DisableBlackHole();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(true);
            attackTargets.Add(aIEnemy);
            aIEnemy.blackHoleAffected = true;
            aIEnemy.blackHolePosition = this.transform;
            aIEnemy.BearMove();
        }
    }
    #endregion

    private void AttackLogic()
    {
        Vector3 lookDirection = Vector3.zero;
        for (int i = 0; i < attackTargets.Count; i++)
        {
            lookDirection = attackTargets[i].transform.position - this.transform.position;
            lookDirection.y = 0;
            attackTargets[i].transform.rotation = Quaternion.LookRotation(lookDirection);

            if (sqrKillRange > Vector3.SqrMagnitude(this.transform.position - attackTargets[i].gameObject.transform.position))
            {
                attackTargets[i].TakeDamage(9999999, AttackType.METEORITE);
                attackTargets.Remove(attackTargets[i]);
            }
        }
    }

    private void DisableBlackHole()
    {
        for (int i = 0; i < attackTargets.Count; i++)
        {
            attackTargets[i].blackHoleAffected = false;
            attackTargets[i].blackHolePosition = null;
        }

        enabled = false;
        if (this.gameObject.GetComponent<BlackHole>() != null)
            GameManager.instance.GetPlayer1().SetIsBlackHoleOn(false);
        ReturnToPool();
    }

    public override void Restart()
    {
        enabled = true;
    }
}
