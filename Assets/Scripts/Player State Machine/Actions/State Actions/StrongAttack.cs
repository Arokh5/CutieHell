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
    [SerializeField]
    private float stunDuration;
    [SerializeField]
    private float knockbackForce;

    private bool holdingButton;
    private float timeHolding;

    public override void Act(Player player)
    {
        if (!player.canChargeStrongAttack)
        {
            if (!InputManager.instance.GetOButton()) player.canChargeStrongAttack = true;
        }

        switch (player.teleportState)
        {
            case Player.JumpStates.JUMP:
                if (player.strongAttackTimer >= timeToGoOut)
                {
                    player.canMove = true;
                    player.strongAttackCollider.Activate();
                    player.strongAttackCollider.SetAttackSize(initialSize);
                    player.teleportState = Player.JumpStates.MOVE;
                    holdingButton = false;
                    timeHolding = 0.0f;
                }
                break;
            case Player.JumpStates.MOVE:
                bool attack = false;
                if (player.canChargeStrongAttack && InputManager.instance.GetOButton())
                {
                    player.isChargingStrongAttack = true;
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
                    ParticleSystem ps = ParticlesManager.instance.LaunchParticleSystem(strongAttackVFX, player.transform.position + player.transform.forward * 0.2f, strongAttackVFX.transform.rotation);
                    StrongAttackDecalSize decalSize = ps.GetComponent<StrongAttackDecalSize>();
                    decalSize.size = initialSize + (timeHolding / timeToHold) * sizeToIncrease * 1.55f;  //The 55% extra is becuse otherwise, the decal wouldn't match with the area
                    decalSize.scale = (initialSize + (timeHolding / timeToHold) * sizeToIncrease) / 4.0f;
                    decalSize.UpdateThis();
                    player.strongAttackMotionLimiter.SetActive(false);
                    player.canMove = false;
                    player.isChargingStrongAttack = false;
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
            aiEnemy.SetKnockback(player.transform.position, knockbackForce);
            aiEnemy.SetStun(stunDuration);
        }
    }
}
