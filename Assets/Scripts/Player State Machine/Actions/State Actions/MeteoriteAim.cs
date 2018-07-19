using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

[CreateAssetMenu(menuName = "Player State Machine/Actions/MeteoriteAim")]
public class MeteoriteAim : StateAction
{
    public int damage;
    public ParticleSystem meteoritePrefab;
    public GameObject meteoriteDestinationPrefab;
    private GameObject meteoriteDestination;
    public float maxSpeed, acceleration;

    public override void Act(Player player)
    {
        Vector3 accelerationVector = Vector3.zero;
        Vector3 verticalAcceleration = player.transform.forward;
        Vector3 horizontalAcceleration = player.transform.right;

        accelerationVector += verticalAcceleration * -InputManager.instance.GetLeftStickVerticalValue();
        accelerationVector += horizontalAcceleration * InputManager.instance.GetLeftStickHorizontalValue();

        float accelerationMagnitude = accelerationVector.magnitude;
        if (accelerationMagnitude > 0.1f)
        {
            if (accelerationMagnitude > 1.0f)
            {
                accelerationVector.Normalize();
                accelerationMagnitude = 1.0f;
            }
            Vector3 playerPos = player.rb.position;
            player.mainCameraController.timeSinceLastAction = 0.0f;
            if (accelerationMagnitude != 0.0f)
            {
                float dot = Vector3.Dot(accelerationVector / accelerationMagnitude, player.currentSpeed);
                if (dot > 0)
                    player.currentSpeed = (accelerationVector / accelerationMagnitude) * dot;
                else
                    player.currentSpeed = Vector3.zero;
            }
            else
                player.currentSpeed = Vector3.zero;

            /* Calculate currentSpeed */
            player.currentSpeed += acceleration * accelerationVector * Time.deltaTime;
            if (player.currentSpeed.sqrMagnitude > maxSpeed * maxSpeed)
                player.currentSpeed = player.currentSpeed.normalized * maxSpeed;

            /* Calculate new position */
            playerPos += player.currentSpeed * Time.deltaTime;

            player.rb.position = playerPos;
        }
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
            player.transform.position = player.initialPos;
        }
    }
}
