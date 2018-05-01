using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/BatTurretAttack")]
public class BatTurretAttack : StateAction
{
    public LayerMask layerMask;
    public LayerMask targetsLayerMask;
    public float sphereCastRadius;
    public float attackCadency;
    public FollowTarget attackPrefab;

    public override void Act(Player player)
    {
        RaycastHit hit;
        bool raycastHit = Physics.SphereCast(Camera.main.transform.position, sphereCastRadius, Camera.main.transform.forward, out hit, 100, layerMask.value);

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
        if (InputManager.instance.GetR2Button() && player.timeSinceLastAttack >= attackCadency)
        {
            if (hitSuccess && HitInEnemyLayer(hitInfo))
            {
                player.InstantiateAttack(attackPrefab, hitInfo.transform, hitInfo.point);
            }
            else
            {
                player.InstantiateAttack(attackPrefab, null, Vector3.zero);
            }
            player.timeSinceLastAttack = 0f;
        }
    }

    private bool HitInEnemyLayer(RaycastHit hit)
    {
        return ((1 << hit.transform.gameObject.layer) & targetsLayerMask) != 0;
    }
}
