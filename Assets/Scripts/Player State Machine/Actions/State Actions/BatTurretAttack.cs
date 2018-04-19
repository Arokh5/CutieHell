using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/BatTurretAttack")]
public class BatTurretAttack : StateAction
{
    public LayerMask layerMask;
    public float sphereCastRadius;
    public float attackCadency;
    public FollowTarget attackPrefab;

    public override void Act(Player player)
    {
        AIEnemy newTarget = null;
        RaycastHit hit;
        bool raycastHit = Physics.SphereCast(Camera.main.transform.position, sphereCastRadius, Camera.main.transform.forward, out hit, 100, layerMask.value);

        /* Targetting */
        if (raycastHit)
        {
            newTarget = hit.transform.GetComponent<AIEnemy>();
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

        /* Shooting */
        if (InputManager.instance.GetR2Button() && player.timeSinceLastAttack >= attackCadency)
        {
            if (raycastHit && hit.transform.GetComponent<AIEnemy>())
            {
                player.InstantiateAttack(attackPrefab, hit.transform, hit.point);
            }
            else
            {
                player.InstantiateAttack(attackPrefab, null, Vector3.zero);
            }
            player.timeSinceLastAttack = 0f;
        }
    }
}
