using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/SummonAttack")]
public class SummonAttack : StateAction
{
    private SummonerTrap summonerTrap;
    private float seductiveLastLaunchedTime = 0f;

    public override void Act(Player player)
    {
        if (!summonerTrap)
        {
            summonerTrap = player.currentTrap.GetComponent<SummonerTrap>();
        }
        FocusSeductiveAttack(player);
        seductiveLastLaunchedTime += Time.deltaTime;

        if (summonerTrap.currentSummoningStatus == SummonerTrap.SummoningStatus.InsufficientEvil)
        {
            if (player.evilLevel >= summonerTrap.summonCost)
            {
                summonerTrap.ChangeCurrentSummoningStatus(SummonerTrap.SummoningStatus.Available);
            }
        }
    }

    private void FocusSeductiveAttack(Player player)
    {
        if (InputManager.instance.GetR2ButtonDown())
        {
            if (player.evilLevel >= summonerTrap.summonCost && seductiveLastLaunchedTime >= summonerTrap.cooldownBetweenSeductiveProjections || summonerTrap.GetLandedEnemyProjectionsCount() == 0)
            {
                summonerTrap.LandSeductiveEnemyProjection();
                summonerTrap.InstantiateSeductiveEnemyProjection();
                seductiveLastLaunchedTime = Time.deltaTime;
                player.SetEvilLevel(-summonerTrap.summonCost);

                if (player.evilLevel < summonerTrap.summonCost)
                {
                    summonerTrap.ChangeCurrentSummoningStatus(SummonerTrap.SummoningStatus.InsufficientEvil);
                }
            }
        }
    }
}

