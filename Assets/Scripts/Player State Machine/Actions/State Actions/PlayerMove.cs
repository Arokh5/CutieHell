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
    public LayerMask walkableLayer;
    public bool canExitWalkableLayer;

    public override void Act(Player player)
    {
        if (player.canMove)
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

            /* Adjust ground height */
            RaycastHit hit;
            float upOffset = 10.0f;
            Vector3 forwardOffsetVector = (accelerationMagnitude > 0 ? (0.25f * accelerationVector / accelerationMagnitude) : Vector3.zero);
            if (Physics.Raycast(player.transform.position + upOffset * Vector3.up + forwardOffsetVector, -Vector3.up, out hit, 50, walkableLayer))
            {
                if (Mathf.Abs(hit.distance - upOffset - player.floorClearance) > 0.005f)
                    playerPos.y -= (hit.distance - upOffset - player.floorClearance);

                player.lastValidPosition = playerPos;
            }
            else if (!canExitWalkableLayer)
            {
                playerPos = player.lastValidPosition;
            }

            /* Set newly calculated position to rigidbody */
            player.rb.position = playerPos;
        }
    }
}
