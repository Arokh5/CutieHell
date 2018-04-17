using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackExplosionDetection : MonoBehaviour {

    #region Fields
    public LayerMask layerMask;
    //[HideInInspector]
    public List<AIEnemy> currentStrongAttackTargets = new List<AIEnemy>();
    #endregion

    #region MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(true);
            currentStrongAttackTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(false);
            currentStrongAttackTargets.Remove(aIEnemy);
        }
    }
    #endregion
}
