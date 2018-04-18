using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttack")]
public class StrongAttack : StateAction
{
    public float strongAttackCadency;
    public int evilCost;
    public int damage;
    public GameObject strongAttackVFX;

    public override void Act(Player player)
    {
        if (player.timeSinceLastStrongAttack >= strongAttackCadency && player.GetEvilLevel() >= Mathf.Abs(evilCost) && !player.animatingAttack)
        {
            player.projector.SwitchToDefaultColor();
            player.strongAttackMeshCollider.enabled = true;

            if (InputManager.instance.GetR2ButtonDown())
            {
                player.animator.SetTrigger("StrongAttack");
                //player.SetEvilLevel(evilCost);
                //GameObject strongAttackExplosion = Instantiate(strongAttackVFX, player.transform.position, player.transform.rotation);
                //strongAttackExplosion.transform.SetParent(player.transform);
                //strongAttackExplosion.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
                //strongAttackExplosion.transform.localRotation = Quaternion.Euler(new Vector3(-90, 180, 0));
                //strongAttackExplosion.transform.SetParent(null);
                //HurtEnemies(player);
            }
        }
        else
        {
            player.projector.SwitchToAlternateColor();
            player.strongAttackMeshCollider.enabled = false;
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
    }
}
