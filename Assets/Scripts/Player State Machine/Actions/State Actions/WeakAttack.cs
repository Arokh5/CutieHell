using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/WeakAttack")]
public class WeakAttack : StateAction
{
    public LayerMask layerMask;
    public float sphereCastRadius;
    public float attackCadency;

    private const int enemiesToBadCombo = 5;
    private const int badComboPenalty = -5;
    private int badComboCount = 0;

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
        if (InputManager.instance.GetR2Button() && player.timeSinceLastAttack >= attackCadency && !player.animatingAttack)
        {
            if (raycastHit && hit.transform.GetComponent<AIEnemy>())
            {
                player.weakAttackTargetHitPoint = hit.point;
                player.weakAttackTargetTransform = hit.transform;
                badComboCount = 0;
            }
            else
            {
                player.weakAttackTargetHitPoint = Vector3.zero;
                player.weakAttackTargetTransform = null;
                badComboCount++;
            }
            player.animator.SetTrigger("Attack");
            player.timeSinceLastAttack = 0f;
            CheckIfBadCombo(player);
        }
    }

    private void CheckIfBadCombo(Player player)
    {
        if (badComboCount == enemiesToBadCombo)
        {
            //Debug.Log("NOOB!!");
            badComboCount = 0;
            player.SetEvilLevel(badComboPenalty);
            UIManager.instance.ShowComboText(UIManager.ComboTypes.BadCombo);
        }
    }
}
