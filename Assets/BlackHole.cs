using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {

    #region Fields
    public LayerMask layerMask;
    public List<AIEnemy> attackTargets = new List<AIEnemy>();
    public float slowRange, killRange;
    public SphereCollider sphereCollider;
    private float sqrKillRange;
    #endregion

    #region MonoBehaviour Methods
    void OnEnable()
    {
        sphereCollider.radius = slowRange;
        attackTargets.Clear();
        sqrKillRange = killRange * killRange;
    }

    void Update()
    {
        AttackLogic();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(true);
            attackTargets.Add(aIEnemy);
            aIEnemy.Freeze();
        }
    }
    #endregion

    private void AttackLogic()
    {
        Vector3 lookDirection = Vector3.zero;
        for (int i = 0; i < attackTargets.Count; i++)
        {
            lookDirection = attackTargets[i].transform.position - this.transform.position;          
            attackTargets[i].transform.rotation = Quaternion.LookRotation(lookDirection);

            if (sqrKillRange > Vector3.SqrMagnitude(this.transform.position - attackTargets[i].gameObject.transform.position))
            {
                attackTargets[i].TakeDamage(9999999, AttackType.METEORITE);
                attackTargets.Remove(attackTargets[i]);
            }
            else
            {
                attackTargets[i].transform.Translate(-lookDirection.normalized * 4 * Time.deltaTime, Space.World);
            }
        }
    }
}
