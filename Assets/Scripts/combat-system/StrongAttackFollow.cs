using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackFollow : MonoBehaviour {

    #region Fields
    public LayerMask layerMask;
    public List<AIEnemy> attackTargets = new List<AIEnemy>();
    private float timer;
    private Player player;
    public int strongAttackFollowDamage;
    #endregion

    #region MonoBehaviour Methods

    private void OnEnable()
    {
        timer = 0.0f;
        player = GameManager.instance.GetPlayer1();
    }

    private void Update()
    {
        if(timer >= 0.1f)
        {
            HurtEnemies(player, strongAttackFollowDamage);
            timer = -10000.0f;
        }
        else
            timer += Time.deltaTime;
        
    }

    private void HurtEnemies(Player player, int damage)
    {
        foreach (AIEnemy aiEnemy in attackTargets)
        {
            aiEnemy.TakeDamage(damage, AttackType.STRONG);
            aiEnemy.SetKnockback(player.transform.position, 1.0f);
            aiEnemy.SetStun(3.0f);
        }
        attackTargets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(true);
            attackTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, layerMask))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            aIEnemy.MarkAsTarget(false);
            attackTargets.Remove(aIEnemy);
        }
    }
    #endregion
}
