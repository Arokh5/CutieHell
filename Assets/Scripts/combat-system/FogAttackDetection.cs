using UnityEngine;

public class FogAttackDetection : MonoBehaviour
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
            if (aIEnemy)
                GameManager.instance.GetPlayer1().currentFogAttackTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            if (aIEnemy)
                GameManager.instance.GetPlayer1().currentFogAttackTargets.Remove(aIEnemy);
        }
    }
    #endregion
}
