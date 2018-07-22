using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields
    public LayerMask layerMask;
    #endregion

    #region MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            if (aIEnemy)
            {
                aIEnemy.MarkAsTarget(true);
                GameManager.instance.GetPlayer1().currentStrongAttackTargets.Add(aIEnemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            if (aIEnemy)
            {
                aIEnemy.MarkAsTarget(false);
                GameManager.instance.GetPlayer1().currentStrongAttackTargets.Remove(aIEnemy);
            }
        }
    }
	#endregion
}
