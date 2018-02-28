using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAttack : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private LayerMask layerMask;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Update()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            Debug.DrawRay(transform.position, player.transform.forward * 100, Color.red, 2);

            if (Physics.Raycast(transform.position, player.transform.forward, 100, layerMask.value))
            {
                Debug.Log("Hit enemy");
            }
        }
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion
}