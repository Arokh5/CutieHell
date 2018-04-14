using System.Collections.Generic;
using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields
    public LayerMask layerMask;
    #endregion

    #region MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(true);
            GameManager.instance.GetPlayer1().currentStrongAttackTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(false);
            GameManager.instance.GetPlayer1().currentStrongAttackTargets.Remove(aIEnemy);
        }
    }
	#endregion
}