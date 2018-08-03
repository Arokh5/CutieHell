using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/DashEnterAction")]
public class DashEnterAction : StateAction
{
    [SerializeField]
    private ParticleSystem dashVFX;

    public override void Act(Player player)
    {
        player.dashInitialPos = player.transform.position;
        ParticleSystem ps = ParticlesManager.instance.LaunchParticleSystem(dashVFX,player.transform.position,dashVFX.transform.rotation);
        ps.transform.SetParent(player.transform);
    }
}
