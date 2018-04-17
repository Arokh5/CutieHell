using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/SummonAttack")]
public class SummonAttack : StateAction
{
    public SummonerTrap summonerTrap;
    public float seductiveLastLaunchedTime = 0f;

    public override void Act(Player player)
    {
        summonerTrap = player.currentTrap.GetComponent<SummonerTrap>();
        Debug.Log("Avoid this assignment in every act frame");
        FocusSeductiveAttack();

        seductiveLastLaunchedTime += Time.deltaTime;
        }

    private void FocusSeductiveAttack()
    {
        if (InputManager.instance.GetR2ButtonDown())
        {
            if (seductiveLastLaunchedTime >= summonerTrap.cooldownBetweenSeductiveProjections|| summonerTrap.GetLandedEnemyProjectionsCount() == 0)
            {    
                summonerTrap.LandSeductiveEnemyProjection();
                summonerTrap.InstantiateSeductiveEnemyProjection();
                seductiveLastLaunchedTime = Time.deltaTime;     
            }
        }
    }
}

