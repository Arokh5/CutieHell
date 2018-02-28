using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAttack : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private LayerMask layerMask;
    private RaycastHit hit;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Update()
    {
        if (InputManager.instance.GetXButtonDown())
        {
            Debug.DrawRay(transform.position, transform.forward * 100, Color.red, 2);

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100, layerMask.value))
            {
                GetComponent<InstantiateAttack>().InstantiateRedOrb(hit.transform);
                Debug.Log("Hit enemy");
            }
            else
            {
                GetComponent<InstantiateAttack>().InstantiateRedOrb(null);
            }
        }
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion
}