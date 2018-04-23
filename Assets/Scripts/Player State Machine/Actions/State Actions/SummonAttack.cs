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

        switch(summonerTrap.currentSummoningStatus)
        {
            case SummonerTrap.SummoningStatus.InsufficientEvil:
                if (player.evilLevel >= summonerTrap.summonCost)
                {
                    summonerTrap.ChangeCurrentSummoningStatus(SummonerTrap.SummoningStatus.Available);
                }
                break;
            case SummonerTrap.SummoningStatus.CoolDown:
                if (seductiveLastLaunchedTime >= summonerTrap.cooldownBetweenSeductiveProjections)
                {
                    summonerTrap.ChangeCurrentSummoningStatus(SummonerTrap.SummoningStatus.Available);
                }
                break;
        }
    }

    private void FocusSeductiveAttack(Player player)
    {
        if (InputManager.instance.GetR2ButtonDown())
        {
            if (summonerTrap.currentSummoningStatus == SummonerTrap.SummoningStatus.Available)
            {
                summonerTrap.LandSeductiveEnemyProjection();
                summonerTrap.InstantiateSeductiveEnemyProjection();
                seductiveLastLaunchedTime = Time.deltaTime;
                player.SetEvilLevel(-summonerTrap.summonCost);

                if (player.evilLevel < summonerTrap.summonCost)
                {
                    summonerTrap.ChangeCurrentSummoningStatus(SummonerTrap.SummoningStatus.InsufficientEvil);
                }
                else
                {
                    summonerTrap.ChangeCurrentSummoningStatus(SummonerTrap.SummoningStatus.CoolDown);
                }
            }
        }
    }
}

