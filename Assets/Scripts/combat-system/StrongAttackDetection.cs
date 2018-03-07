using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackDetection : MonoBehaviour
{
    #region Fields

    private const float strongAttackCadency = 3f;
    private const float actionTimeRange = 0.5f;
    private float time = strongAttackCadency;
    private float actionTime = 0f;
    private bool activateAttack = false;

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Update()
    {
        StrongAttackActivation();
    }

    private void OnTriggerStay(Collider other)
    {
        if (activateAttack)
        {
            if (other.gameObject.layer == 8)
            {
                Destroy(other.gameObject);
            }
        }
    }

	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Private Methods

    private void StrongAttackActivation()
    {
        time += Time.deltaTime;

        if (InputManager.instance.GetL2Button())
        {
            if (time >= strongAttackCadency)
            {
                if (actionTime < actionTimeRange)
                {
                    activateAttack = true;
                    GetComponent<MeshCollider>().enabled = true;
                    GetComponent<Renderer>().enabled = true;
                }
            }
        }

        if (activateAttack)
        {
            actionTime += Time.deltaTime;
        }

        if (actionTime >= actionTimeRange)
        {
            activateAttack = false;
            time = 0f;
            actionTime = 0f;
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }
    }

	#endregion
}