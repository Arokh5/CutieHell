using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttack")]
public class StrongAttack : StateAction
{
    public float strongAttackCadency;
    public int evilCost;
    public int damage;

    public override void Act(Player player)
    {
        if (player.timeSinceLastStrongAttack >= strongAttackCadency && player.GetEvilLevel() >= Mathf.Abs(evilCost))
        {
            player.strongAttackMeshCollider.enabled = true;
            player.strongAttackRenderer.enabled = true;

            if (InputManager.instance.GetR2ButtonDown())
            {
                player.SetEvilLevel(evilCost);
                HurtEnemies(player);
            }
        }
        else
        {
            player.strongAttackMeshCollider.enabled = false;
            player.strongAttackRenderer.enabled = false;
        }
    }

    private void HurtEnemies(Player player)
    {
        foreach (AIEnemy aiEnemy in player.currentStrongAttackTargets)
        {
            aiEnemy.MarkAsTarget(false);
            aiEnemy.TakeDamage(damage, AttackType.STRONG);
        }
        player.currentStrongAttackTargets.Clear();

        player.timeSinceLastStrongAttack = 0f;
        player.strongAttackMeshCollider.enabled = false;
        player.strongAttackRenderer.enabled = false;
    }
}
