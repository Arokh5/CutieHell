using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/BasicAttack")]
public class BasicAttack : StateAction {
    public BasicAttackData attackData;

    public override void Act(Player player)
    {
        player.timeSinceLastAttack += Time.deltaTime;

        AIEnemy newTarget = null;
        RaycastHit hit;
        bool raycastHit = Physics.SphereCast(Camera.main.transform.position, attackData.sphereCastRadius, Camera.main.transform.forward, out hit, 100, attackData.layerMask.value);

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
        if (InputManager.instance.GetR2Button() && player.timeSinceLastAttack >= attackData.attackCadency)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red, 2);

            if (raycastHit && hit.transform.GetComponent<AIEnemy>())
                InstantiateAttack(player, hit.transform, hit.point);
            else
                InstantiateAttack(player, null, Vector3.zero);

            player.timeSinceLastAttack = 0f;
        }
    }

    private void InstantiateAttack(Player player, Transform enemy, Vector3 hitPoint)
    {
        Vector3 spawningPos = player.bulletSpawnPoint.position;

        FollowTarget attackClone = Instantiate(attackData.attackPrefab, spawningPos, player.transform.rotation);
        attackClone.SetEnemy(enemy);
        attackClone.SetHitPoint(hitPoint);
    }
}
