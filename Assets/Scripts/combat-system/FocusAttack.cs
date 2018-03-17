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

    private void FixedUpdate()
    {
        foreach (AIEnemy enemy in FindObjectsOfType<AIEnemy>())
        {
            enemy.MarkAsTarget(false);
        }
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
                    GetComponent<AttackController>().InstantiateAttack(hit.transform, hit.point);
                    Debug.Log("Hit enemy");
                }
                else
                {
                    GetComponent<AttackController>().InstantiateAttack(null, Vector3.zero);
                }

                time = 0f;
            }
        }

        if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, 100, layerMask.value) && !InputManager.instance.GetL2Button())
        {
            hit.transform.GetComponent<AIEnemy>().MarkAsTarget(true);
        }
    }

    #endregion
}