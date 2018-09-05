using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttack")]
public class StrongAttack : StateAction
{
    public float damage;
    public ParticleSystem strongAttackVFX;
    public float timeToGoOut, timeToGoIn, delay;
    public float timeToHold;
    private bool holdingButton;
    private float timeHolding;

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
                    holdingButton = false;
                    timeHolding = 0.0f;
                }
                break;
            case Player.TeleportStates.TRAVEL:
                bool attack = false;
                if (InputManager.instance.GetOButton())
                {
                    holdingButton = true;
                    timeHolding += Time.deltaTime;

                    if (timeHolding > timeToHold)
                    {
                        timeHolding = timeToHold;
                        attack = true;
                    }

                    player.IncreaseStrongAttackColliderSize(timeHolding);
                    player.ChangeDecalColor(timeHolding / timeToHold);
                }
                else
                {
                    if (holdingButton)
                        attack = true;
                }

                if (attack)
                {
                    player.ResetStrongAttackColliderSize();
                    player.teleportState = Player.TeleportStates.IN;
                    player.strongAttackTimer = 0.0f;
                    ParticlesManager.instance.LaunchParticleSystem(strongAttackVFX, player.transform.position, strongAttackVFX.transform.rotation);
                    player.strongAttackCollider.Deactivate();
                    player.strongAttackMotionLimiter.SetActive(false);
                    player.canMove = false;
                    player.animator.Rebind();
                }
                break;
            case Player.TeleportStates.IN:
                if (player.strongAttackTimer >= timeToGoIn)
                {
                    BulletTime.instance.DoSlowmotion(0.01f, 0.25f);
                    CameraShaker.Instance.ShakeOnce(0.8f, 15.5f, 0.1f, 0.7f);
                    player.cameraState = Player.CameraState.MOVE;
                    player.mainCameraController.y = 5.0f;
                    player.strongAttackTimer = 0.0f;
                    player.teleported = true;
                    player.teleportState = Player.TeleportStates.DELAY;
                    player.strongAttackCooldown.timeSinceLastAction = 0.0f;
                    damage *= 1f + timeHolding / timeToHold;
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
    private void HurtEnemies(Player player, float damage)
    {
        foreach (AIEnemy aiEnemy in player.currentStrongAttackTargets)
        {
            aiEnemy.TakeDamage(damage, AttackType.STRONG);
            aiEnemy.SetKnockback(player.transform.position, 3.0f);
            aiEnemy.SetStun(3.0f);
        }
    }
}
