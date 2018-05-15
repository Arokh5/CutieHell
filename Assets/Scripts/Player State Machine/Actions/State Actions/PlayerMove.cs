using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlayerMove")]
public class PlayerMove : StateAction
{
    public float maxSpeed;
    public float acceleration;
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
            player.footSteps.SetActive(true);
            player.animator.SetBool("Move", true);
            if (!player.footstepsSource.isPlaying)
            {
                player.footstepsSource.Play();
            }

        }
        else
        {
            player.rb.drag = 10f;
            player.rb.angularDrag = 10f;
            player.footSteps.SetActive(false);
            player.animator.SetBool("Move", false);
            player.footstepsSource.Stop();
        }

        player.rb.AddRelativeForce(speedDirection * acceleration, ForceMode.Acceleration);

        //if (player.rb.velocity.magnitude > player.maxSpeed)
        //{
        //    player.rb.velocity = player.rb.velocity.normalized * player.maxSpeed;
        //}
        if (player.rb.velocity.magnitude > maxSpeed * speedDirection.magnitude / 2.0f)
        {
            player.rb.velocity = player.rb.velocity.normalized * maxSpeed * speedDirection.magnitude / 2.0f;
        }
    }
}
