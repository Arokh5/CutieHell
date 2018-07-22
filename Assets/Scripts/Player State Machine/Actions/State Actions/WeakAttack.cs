using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/WeakAttack")]
public class WeakAttack : StateAction
{
    public LayerMask layerMask;
    public LayerMask targetsLayerMask;
    public float sphereCastRadius;
    public float attackCadency;
    public bool allowMultipleAttacks = false;

    [SerializeField]
    private AudioClip attackSfx;

    public override void Act(Player player)
    {
        RaycastHit hit;
        Vector3 rayStart = player.mainCameraController.transform.position + player.mainCameraController.transform.forward * player.mainCameraController.distance;
        bool raycastHit = Physics.SphereCast(rayStart, sphereCastRadius, Camera.main.transform.forward, out hit, 100, layerMask.value);

        UpdatePlayerTarget(player, ref raycastHit, hit);
        Shoot(player, raycastHit, hit);
    }

    private void UpdatePlayerTarget(Player player, ref bool hitSuccess, RaycastHit hitInfo)
    {
        AIEnemy newTarget = null;
        if (hitSuccess && HitInEnemyLayer(hitInfo))
        {
            newTarget = hitInfo.transform.GetComponent<AIEnemy>();
            if (!newTarget.GetIsTargetable())
            {
                newTarget = null;
                hitSuccess = false;
            }
        }

        if (player.currentBasicAttackTarget)
        {
            if (!newTarget)
            {
                player.currentBasicAttackTarget.MarkAsTarget(false);
                player.currentBasicAttackTarget = null;
            }
            else if (player.currentBasicAttackTarget != newTarget)
            {
                player.currentBasicAttackTarget.MarkAsTarget(false);
                newTarget.MarkAsTarget(true);
                player.currentBasicAttackTarget = newTarget;
            }
        }
        else if (newTarget)
        {
            newTarget.MarkAsTarget(true);
            player.currentBasicAttackTarget = newTarget;
        }
    }

    private void Shoot(Player player, bool hitSuccess, RaycastHit hitInfo)
    {
        if (InputManager.instance.GetR2Button() 
            && player.timeSinceLastAttack >= attackCadency 
            && !player.animatingAttack 
            && (allowMultipleAttacks || player.basicAttacksCount == 0)
            )
        {
            SoundManager.instance.PlaySfxClip(attackSfx, 1.5f);

            if (hitSuccess && HitInEnemyLayer(hitInfo))
            {
                player.weakAttackTargetHitOffset = hitInfo.point - hitInfo.transform.position;
                player.weakAttackTargetTransform = hitInfo.transform;
            }
            else
            {
                player.weakAttackTargetHitOffset = Vector3.zero;
                player.weakAttackTargetTransform = null;
            }
            player.animator.SetTrigger("Attack");
            player.timeSinceLastAttack = 0f;
        }
    }

    private bool HitInEnemyLayer(RaycastHit hit)
    {
        return Helpers.GameObjectInLayerMask(hit.transform.gameObject, targetsLayerMask);
    }
}
