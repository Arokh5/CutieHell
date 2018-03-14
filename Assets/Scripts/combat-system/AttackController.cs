using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject mainAttack;
    [SerializeField]
    private GameObject batAttack;

	#endregion
	
	#region Properties
	
    #endregion
	
	#region MonoBehaviour Methods
	
	#endregion
	
	#region Public Methods
	
    public void InstantiateAttack(Transform enemy)
    {
        GameObject attack = (GameManager.instance.GetPlayer1().state == Player.PlayerStates.TURRET) ? batAttack : mainAttack;

        GameObject attackClone = Instantiate(attack, transform.GetChild(0).position, transform.rotation);
        attackClone.GetComponent<FollowTarget>().SetEnemy(enemy);
    }

	#endregion
	
	#region Private Methods
	
	#endregion
}