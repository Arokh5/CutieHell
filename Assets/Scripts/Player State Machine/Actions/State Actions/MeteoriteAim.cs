using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

[CreateAssetMenu(menuName = "Player State Machine/Actions/MeteoriteAim")]
public class MeteoriteAim : StateAction
{
    public ParticleSystem meteoritePrefab;

    public override void Act(Player player)
    {
        RaycastHit hit;
        int layerMask = 1 << 17;
        if (Physics.Raycast(player.mainCamera.transform.position, player.mainCamera.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            player.lastMeteoriteAttackDestination = hit.point + Vector3.up;
            player.meteoriteDestinationMarker.SetActive(true);
            player.meteoriteDestinationMarker.transform.position = player.lastMeteoriteAttackDestination;
        }
        else
        {
            player.meteoriteDestinationMarker.SetActive(false);
        }
        if (InputManager.instance.GetTriangleButtonDown() && player.meteoriteDestinationMarker.activeSelf)
        {
            ParticlesManager.instance.LaunchParticleSystem(meteoritePrefab, player.lastMeteoriteAttackDestination, meteoritePrefab.transform.rotation);
            player.comeBackFromMeteoriteAttack = true;
            player.transform.position = player.meteoritesReturnPlayerPosition[player.currentZonePlaying].position;
            player.meteoriteAttackCooldown.timeSinceLastAction = 0.0f;
            player.mainCameraController.y = 0.0f;
        }
        else if (InputManager.instance.GetOButtonDown())
        {
            player.comeBackFromMeteoriteAttack = true;
            player.transform.position = player.meteoritesReturnPlayerPosition[player.currentZonePlaying].position;
            player.mainCameraController.y = 0.0f;
        }
    }
}
