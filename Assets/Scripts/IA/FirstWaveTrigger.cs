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
        if (((1 << other.gameObject.layer) & triggerLayerMask) != 0)
        {
            gameObject.SetActive(false);
            GameManager.instance.StartNextRound();
        }
    }
    #endregion
}
