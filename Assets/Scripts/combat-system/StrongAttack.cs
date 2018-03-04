using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : MonoBehaviour
{
	#region Fields
	
	#endregion
	
	#region Properties
	
    #endregion
	
	#region MonoBehaviour Methods
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("Enemy name: " + other.gameObject.name);
        }
    }

	#endregion
	
	#region Public Methods
	
	#endregion
	
	#region Private Methods
	
	#endregion
}