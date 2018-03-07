using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAttack : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private LayerMask layerMask;

    private const float sphereCastRadius = 0.5f;
    private const float basicAttackCadency = 0.5f;

    private RaycastHit hit;
    private float time = basicAttackCadency;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Update()
    {
        FocusBasicAttack();
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void FocusBasicAttack()
    {
        time += Time.deltaTime;

        if (InputManager.instance.GetR2Button())
        {
            if (time >= basicAttackCadency)
            {
                Debug.DrawRay(transform.position, transform.forward * 100, Color.red, 2);

                if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, 100, layerMask.value))
                {
                    GetComponent<InstantiateAttack>().InstantiateRedOrb(hit.transform);
                    Debug.Log("Hit enemy");
                }
                else
                {
                    GetComponent<InstantiateAttack>().InstantiateRedOrb(null);
                }

                time = 0f;
            }
        }
    }

    #endregion
}