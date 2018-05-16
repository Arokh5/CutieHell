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
        Vector3 speedDirection = Vector3.zero;
        Vector3 verticalAcceleration = new Vector3(0f, 0f, 1.5f);
        Vector3 horizontalAcceleration = new Vector3(1.5f, 0f, 0f);

        if (InputManager.instance.GetLeftStickUp())
        {
            speedDirection += verticalAcceleration * -InputManager.instance.GetLeftStickVerticalValue();
        }
        if (InputManager.instance.GetLeftStickDown())
        {
            speedDirection += verticalAcceleration * -InputManager.instance.GetLeftStickVerticalValue();
        }
        if (InputManager.instance.GetLeftStickLeft())
        {
            speedDirection += horizontalAcceleration * InputManager.instance.GetLeftStickHorizontalValue();
        }
        if (InputManager.instance.GetLeftStickRight())
        {
            speedDirection += horizontalAcceleration * InputManager.instance.GetLeftStickHorizontalValue();
        }
        
        if (speedDirection.magnitude > 0.2f)
        {
            player.rb.drag = 0.5f;
            if (useAnimation)
                player.animator.SetBool("Move", true);

            if (useFootsteps)
            {
                player.footSteps.SetActive(true);
                if (!player.footstepsSource.isPlaying)
                    player.footstepsSource.Play();
            }
        }
        else
        {
            player.rb.drag = 10f;
            player.rb.angularDrag = 10f;
            if (useAnimation)
                player.animator.SetBool("Move", false);

            if (useFootsteps)
            {
                player.footSteps.SetActive(false);
                player.footstepsSource.Stop();
            }
        }

        player.rb.AddRelativeForce(speedDirection * acceleration, ForceMode.Acceleration);
        if (player.rb.velocity.magnitude > maxSpeed * speedDirection.magnitude / 2.0f)
        {
            player.rb.velocity = player.rb.velocity.normalized * maxSpeed * speedDirection.magnitude / 2.0f;
        }
    }
}
