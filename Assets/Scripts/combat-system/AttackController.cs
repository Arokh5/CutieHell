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
    [SerializeField]
    private Transform basicTrap;

	#endregion
	
	#region Properties
	
    #endregion
	
	#region MonoBehaviour Methods
	
	#endregion
	
	#region Public Methods
	
    public void InstantiateAttack(Transform enemy, Vector3 hitPoint)
    {
        GameObject attack = (GameManager.instance.GetPlayer1().state == Player.PlayerStates.TURRET) ? batAttack : mainAttack;
        Vector3 spawningPos = (GameManager.instance.GetPlayer1().state == Player.PlayerStates.TURRET) ? basicTrap.GetChild(0).position : transform.GetChild(1).position;

        GameObject attackClone = Instantiate(attack, spawningPos, transform.rotation);
        attackClone.GetComponent<FollowTarget>().SetEnemy(enemy);
        attackClone.GetComponent<FollowTarget>().SetHitPoint(hitPoint);
    }

	#endregion
}