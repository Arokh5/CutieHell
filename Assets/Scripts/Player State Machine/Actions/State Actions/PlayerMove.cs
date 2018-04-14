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

        if (InputManager.instance.GetLeftStickUp())
        {
            speedDirection += new Vector3(0.0f, 0.0f, 0.5f);
        }
        if (InputManager.instance.GetLeftStickDown())
        {
            speedDirection += new Vector3(0.0f, 0.0f, -0.5f);
        }
        if (InputManager.instance.GetLeftStickLeft())
        {
            speedDirection += new Vector3(-0.5f, 0.0f, 0.0f);
        }
        if (InputManager.instance.GetLeftStickRight())
        {
            speedDirection += new Vector3(0.5f, 0.0f, 0.0f);
        }

        if (speedDirection.magnitude > 0.0f)
        {
            player.rb.drag = 0.0f;
            player.footSteps.SetActive(true);
        }
        else
        {
            player.rb.drag = 10.0f;
            player.footSteps.SetActive(false);
        }

        player.rb.AddRelativeForce(speedDirection * player.acceleration, ForceMode.Acceleration);

        if (player.rb.velocity.magnitude > player.maxSpeed)
        {
            player.rb.velocity = player.rb.velocity.normalized * player.maxSpeed;
        }
    }
}
