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
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(true);
            currentStrongAttackTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(false);
            currentStrongAttackTargets.Remove(aIEnemy);
        }
    }
    #endregion
}
