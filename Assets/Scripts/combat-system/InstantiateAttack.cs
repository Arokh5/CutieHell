using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAttack : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject mainAttack;

	#endregion
	
	#region Properties
	
    #endregion
	
	#region MonoBehaviour Methods
	
	#endregion
	
	#region Public Methods
	
    public void InstantiateRedOrb(Transform enemy)
    {
        GameObject mainAttackClone = Instantiate(mainAttack, transform.GetChild(0).position, transform.rotation * Quaternion.Euler(0f, -90f, 0f));
        mainAttackClone.GetComponent<FollowTarget>().SetEnemy(enemy);
    }

	#endregion
	
	#region Private Methods
	
	#endregion
}