using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttackDetection : MonoBehaviour
{
    #region Fields
    public LayerMask layerMask;
    //[HideInInspector]
    public List<AIEnemy> attackTargets = new List<AIEnemy>();
    #endregion

    #region MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(true);
            attackTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(false);
            attackTargets.Remove(aIEnemy);
        }
    }
    #endregion
}
