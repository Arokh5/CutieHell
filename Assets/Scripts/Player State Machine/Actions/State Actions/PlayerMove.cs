using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlayerMove")]
public class PlayerMove : StateAction
{
    public float maxSpeed;
    [HideInInspector]
    public float tempMaxSpeed;
    public float acceleration;
    public bool useAnimation;
    public LayerMask walkableLayer;
    public bool canExitWalkableLayer;

    public override void Act(Player player)
    {
        if (player.canMove)
        {
            Vector3 playerPos = player.rb.position;

            if (player.knockbackActive)
                Knockback(player, ref playerPos);
            else
                Move(player, ref playerPos);

            CheckGround(player, ref playerPos);

            /* Set newly calculated position to rigidbody */
            player.rb.position = playerPos;
        }
    }

    private void Move(Player player, ref Vector3 playerPos)
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
        }
        else
        {
            accelerationVector = Vector3.zero;
            accelerationMagnitude = 0.0f;
            if (useAnimation)
                player.animator.SetBool("Move", false);

        }

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
    }

    private void Knockback(Player player, ref Vector3 playerPos)
    {
        player.animator.SetBool("Move", false);
        player.knockbackCurrentForce = Mathf.Lerp(player.knockbackCurrentForce, 0.0f, 0.4f);
        playerPos += player.knockbackDirection * player.knockbackCurrentForce * Time.deltaTime;
        if (player.knockbackCurrentForce < 0.1f * player.knockbackForce)
        {
            player.knockbackActive = false;
        }
    }

    private void CheckGround(Player player, ref Vector3 playerPos)
    {
        /* Adjust ground height */
        RaycastHit hit;
        float upOffset = 10.0f;
        //if (Physics.Raycast(player.transform.position + upOffset * Vector3.up + inPlaneOffset, -Vector3.up, out hit, 50, walkableLayer))
        if (Physics.Raycast(playerPos + upOffset * Vector3.up, -Vector3.up, out hit, 50, walkableLayer))
        {
            if (Mathf.Abs(hit.distance - upOffset - player.floorClearance) > 0.005f)
                playerPos.y -= (hit.distance - upOffset - player.floorClearance);

            player.lastValidPosition = playerPos;
        }
        else if (!canExitWalkableLayer)
        {
            playerPos = player.lastValidPosition;
        }
    }
}
