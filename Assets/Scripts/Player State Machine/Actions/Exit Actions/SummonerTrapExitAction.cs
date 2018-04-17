using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player State Machine/Actions/SummonerTrapExitAction")]
public class SummonerTrapExitAction : StateAction
{
    public SummonerTrap summonerTrap;

    public override void Act(Player player)
    {
        summonerTrap = player.currentTrap.GetComponent<SummonerTrap>();
        summonerTrap.trapBasicSummonerBeam.gameObject.SetActive(false);
        summonerTrap.seductiveTrapActiveArea.gameObject.SetActive(false);
        GameObject.Destroy(summonerTrap.GetNonLandedProjection());
        player.currentTrap.Deactivate();

        player.shouldExitTrap = false;
        player.meshRenderer.enabled = true;
        player.mainCamera.transform.position = player.transform.position;

        Vector3 nextPos = player.currentTrap.transform.forward * 3f;
        player.transform.position = new Vector3(player.currentTrap.transform.position.x - nextPos.x, player.transform.position.y, player.currentTrap.transform.position.z - nextPos.z);
        player.currentTrap = null;
        player.timeSinceLastTrapUse = 0f;
    }
}
