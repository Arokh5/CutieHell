using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAttack : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float sphereCastRadius;
    [SerializeField]
    private float basicAttackCadency;

    private RaycastHit hit;
    private float time;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        time = basicAttackCadency;
    }

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

        if (InputManager.instance.GetR2Button() && !InputManager.instance.GetL2Button())
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