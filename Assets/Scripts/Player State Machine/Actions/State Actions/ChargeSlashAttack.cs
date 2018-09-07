using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/ConeAttackCharge")]
public class ChargeSlashAttack : StateAction
{
    private float timer = 0.0f;
    public float timeToCharge;

    public override void Act(Player player)
    {
        timer += Time.deltaTime;
        if (!InputManager.instance.GetSquareButton())
        {
            player.slashButtonReleased = true;
            timer = 0.0f;
            player.isDoubleSlash = false;
        }
        if (timer >= timeToCharge)
        {
            player.slashButtonReleased = true;
            timer = 0.0f;
            player.isDoubleSlash = true;
        }             
    }
}
