using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

[CreateAssetMenu(menuName = "Player State Machine/Actions/StrongAttack")]
public class StrongAttack : StateAction
{
    public float minDamage, maxDamage;      
    public float timeToHold;
    public float initialSize;
    public float sizeToIncrease;
    public ParticleSystem strongAttackVFX;
    public float timeToGoOut, timeToGoIn, delay;

    private bool holdingButton;
    private float timeHolding;

    public override void Act(Player player)
    {
        if (!player.canCharge)
        {
            if (!InputManager.instance.GetOButton()) player.canCharge = true;
        }

        switch (player.teleportState)
        {
            case Player.JumpStates.JUMP:
                if (player.strongAttackTimer >= timeToGoOut)
                {
                    player.canMove = true;
                    player.strongAttackCollider.Activate();
                    player.teleportState = Player.JumpStates.MOVE;
                    holdingButton = false;
                    timeHolding = 0.0f;
                }
                break;
            case Player.JumpStates.MOVE:
                bool attack = false;
                if (player.canCharge && InputManager.instance.GetOButton())
                {
                    holdingButton = true;
                    timeHolding += Time.deltaTime;

                    if (timeHolding > timeToHold)
                    {
                        timeHolding = timeToHold;
                        attack = true;
                    }
                    float holdPercent = timeHolding / timeToHold;
                    player.IncreaseStrongAttackColliderSize(holdPercent * sizeToIncrease);
                    player.ChangeDecalColor(holdPercent);
                }
                else
                {
                    if (holdingButton)
                        attack = true;
                }

                if (attack)
                {
                    player.strongAttackCollider.Deactivate();
                    player.ResetStrongAttackColliderSize();
                    player.teleportState = Player.JumpStates.LAND;
                    player.strongAttackTimer = 0.0f;
                    ParticlesManager.instance.LaunchParticleSystem(strongAttackVFX, player.transform.position, strongAttackVFX.transform.rotation);
                    player.strongAttackMotionLimiter.SetActive(false);
                    player.canMove = false;
                    player.animator.Rebind();
                }
                break;
            case Player.JumpStates.LAND:
                if (player.strongAttackTimer >= timeToGoIn)
                {

                    BulletTime.instance.DoSlowmotion(0.01f, 0.25f);
                    CameraShaker.Instance.ShakeOnce(0.8f, 15.5f, 0.1f, 0.7f);
                    player.cameraState = Player.CameraState.MOVE;
                    player.mainCameraController.y = 5.0f;
                    player.strongAttackTimer = 0.0f;
                    player.teleported = true;
                    player.teleportState = Player.JumpStates.DELAY;
                    player.strongAttackCooldown.timeSinceLastAction = 0.0f;
                    float damage = minDamage + ((maxDamage - minDamage) * timeHolding / timeToHold);
                    HurtEnemies(player, damage);
                }
                break;
            case Player.JumpStates.DELAY:
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
