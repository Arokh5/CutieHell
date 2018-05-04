using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/PlayerMove")]
public class PlayerMove : StateAction
{
    public float maxSpeed = 5;

    public override void Act(Player player)
    {
        Vector3 speedDirection = Vector3.zero;
        Vector3 verticalSpeed = new Vector3(0f, 0f, 0.5f);
        Vector3 horizontalSpeed = new Vector3(0.5f, 0f, 0f);

        if (InputManager.instance.GetLeftStickUp())
        {
            speedDirection += verticalSpeed * -InputManager.instance.GetLeftStickVerticalSqrValue();
        }
        if (InputManager.instance.GetLeftStickDown())
        {
            speedDirection += verticalSpeed * -InputManager.instance.GetLeftStickVerticalSqrValue();
        }
        if (InputManager.instance.GetLeftStickLeft())
        {
            speedDirection += horizontalSpeed * InputManager.instance.GetLeftStickHorizontalSqrValue();
        }
        if (InputManager.instance.GetLeftStickRight())
        {
            speedDirection += horizontalSpeed * InputManager.instance.GetLeftStickHorizontalSqrValue();
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

        player.rb.AddRelativeForce(speedDirection * player.acceleration, ForceMode.Acceleration);

        if (player.rb.velocity.magnitude > player.maxSpeed)
        {
            player.rb.velocity = player.rb.velocity.normalized * player.maxSpeed;
        }
    }
}
