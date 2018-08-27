using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstWaveTrigger : MonoBehaviour
{
    #region Fields
    public LayerMask triggerLayerMask;
    #endregion

    #region MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, triggerLayerMask))
        {
            gameObject.SetActive(false);
            GameManager.instance.GoToNextRound();
        }
    }
    #endregion
}
