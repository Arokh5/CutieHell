using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlayerMove")]
public class PlayerMove : StateAction
{
    public float maxSpeed;
    public float acceleration;
    public bool useAnimation;
    public bool useFootsteps;

    public override void Act(Player player)
    {
        Vector3 accelerationVector = Vector3.zero;
        Vector3 verticalAcceleration = player.transform.forward;
        Vector3 horizontalAcceleration = player.transform.right;

        accelerationVector += verticalAcceleration * -InputManager.instance.GetLeftStickVerticalValue();
        accelerationVector += horizontalAcceleration * InputManager.instance.GetLeftStickHorizontalValue();

        float accelerationMagnitude = accelerationVector.magnitude;
        if (accelerationMagnitude > 1.0f)
        {
            accelerationVector.Normalize();
            accelerationMagnitude = 1.0f;
        }

        if (accelerationMagnitude > 0.1f)
        {
            if (useAnimation)
                player.animator.SetBool("Move", true);
            else
                player.mainCameraController.timeSinceLastAction = 0.0f;

            if (useFootsteps)
            {
                player.footSteps.SetActive(true);
                if (!player.footstepsSource.isPlaying)
                    player.footstepsSource.Play();
            }
        }
        else
        {
            accelerationVector = Vector3.zero;
            accelerationMagnitude = 0.0f;
            if (useAnimation)
                player.animator.SetBool("Move", false);

            if (useFootsteps)
            {
                player.footSteps.SetActive(false);
                player.footstepsSource.Stop();
            }
        }

        Vector3 playerPos = player.rb.position;

        /* Remove currentSpeed components that are not aligned with acceleration */
        if (accelerationMagnitude != 0.0f)
            player.currentSpeed = (accelerationVector / accelerationMagnitude) * Vector3.Dot(accelerationVector / accelerationMagnitude, player.currentSpeed);
        else
            player.currentSpeed = Vector3.zero;

        /* Calculate currentSpeed */
        player.currentSpeed += acceleration * accelerationVector * Time.deltaTime;
        if (player.currentSpeed.sqrMagnitude > maxSpeed * maxSpeed)
            player.currentSpeed = player.currentSpeed.normalized * maxSpeed;

        /* Calculate new position */
        playerPos += player.currentSpeed * Time.deltaTime;

        /* Adjust ground height */
        RaycastHit hit;
        int testOffset = 5;
        if (Physics.Raycast(player.transform.position + testOffset * Vector3.up, -Vector3.up, out hit, 50, (1 << 9)))
        {
            if (Mathf.Abs(hit.distance - testOffset - player.floorClearance) > 0.001f)
                playerPos.y -= (hit.distance - testOffset - player.floorClearance);
        }

        /* Set newly calculated position to rigidbody */
        player.rb.position = playerPos;
    }
}
