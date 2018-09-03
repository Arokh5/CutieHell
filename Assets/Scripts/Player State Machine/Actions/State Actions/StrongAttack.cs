using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttack")]
public class StrongAttack : StateAction
{
    public int damage;
    public ParticleSystem strongAttackVFX;
    public float timeToGoOut, timeToGoIn, delay;

    public override void Act(Player player)
    {
        switch (player.teleportState)
        {
            case Player.TeleportStates.OUT:
                if (player.strongAttackTimer >= timeToGoOut)
                {
                    player.canMove = true;
                    player.strongAttackCollider.Activate();
                    player.teleportState = Player.TeleportStates.TRAVEL;
                }
                break;
            case Player.TeleportStates.TRAVEL:
                if (InputManager.instance.GetOButtonDown())
                {
                    player.teleportState = Player.TeleportStates.IN;
                    player.strongAttackTimer = 0.0f;
                    ParticlesManager.instance.LaunchParticleSystem(strongAttackVFX, player.transform.position, strongAttackVFX.transform.rotation);
                    player.strongAttackCollider.Deactivate();
                    player.strongAttackMotionLimiter.SetActive(false);
                    player.canMove = false;
                    player.animator.Rebind();
                    AttackChainsManager.instance.ReportStartChainAttempt(AttackType.STRONG);
                }
                break;
            case Player.TeleportStates.IN:

                if (player.strongAttackTimer >= timeToGoIn)
                {
                    BulletTime.instance.DoSlowmotion(0.01f, 0.25f);
                    CameraShaker.Instance.ShakeOnce(0.8f, 15.5f, 0.1f, 0.7f);
                    player.cameraState = Player.CameraState.MOVE;
                    player.mainCameraController.y = 10.0f;
                    player.strongAttackTimer = 0.0f;
                    player.teleported = true;
                    player.teleportState = Player.TeleportStates.DELAY;
                    player.strongAttackCooldown.timeSinceLastAction = 0.0f;
                    HurtEnemies(player, damage);
                }
                break;
            case Player.TeleportStates.DELAY:
                if (player.strongAttackTimer >= delay)
                {
                    player.comeBackFromStrongAttack = true;
                }   
                break;
            default:
                break;
        }
    }
    private void HurtEnemies(Player player, int damage)
    {
        foreach (AIEnemy aiEnemy in player.currentStrongAttackTargets)
        {
            aiEnemy.TakeDamage(damage, AttackType.STRONG);
            aiEnemy.SetKnockback(player.transform.position, 3.0f);
            aiEnemy.SetStun(3.0f);
        }
    }
}
