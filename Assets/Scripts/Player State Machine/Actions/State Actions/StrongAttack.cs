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

    [SerializeField]
    private AudioClip attackSfx;

    public override void Act(Player player)
    {
        if (player.timeSinceLastStrongAttack >= strongAttackCadency && player.GetEvilLevel() >= Mathf.Abs(evilCost) && !player.animatingAttack)
        {
            player.projector.SwitchToDefaultColor();
            player.strongAttackMeshCollider.enabled = true;

            if (InputManager.instance.GetR2ButtonDown())
            {
                player.animator.SetTrigger("StrongAttack");
                SoundManager.instance.PlaySfxClip(attackSfx);
            }
        }
        else
        {
            player.projector.SwitchToAlternateColor();
            player.strongAttackMeshCollider.enabled = false;
        }
        /* Player rotation */
        player.mainCameraController.timeSinceLastAction = 0.0f;
        player.mainCameraController.fastAction = true;
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
