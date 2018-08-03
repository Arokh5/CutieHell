using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/DashEnterDecision")]
public class DashEnterDecision : Decision
{
    public LayerMask dashLimitLayerMask;

    public override bool Decide(Player player)
    {
        if (InputManager.instance.GetL2ButtonDown())
        {
            Vector3 direction = player.transform.forward;

            float threshold = InputManager.joystickThreshold;
            float verticalValue = -InputManager.instance.GetLeftStickVerticalValue();
            if (verticalValue > -threshold && verticalValue < threshold)
                verticalValue = 0.0f;
            float horizontalValue = InputManager.instance.GetLeftStickHorizontalValue();
            if (horizontalValue > -threshold && horizontalValue < threshold)
                horizontalValue = 0.0f;

            if (horizontalValue != 0.0f || verticalValue != 0.0f)
            {
                direction = new Vector3(horizontalValue, 0, verticalValue);
                direction = player.mainCameraController.transform.TransformDirection(direction);
                direction.Normalize();
            }

            player.dashDirection = direction;
            player.dashElapsedTime = 0.0f;

            // Calculate limit distance
            float upwardsOffset = 1.0f;
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position + upwardsOffset * Vector3.up, player.dashDirection, out hit, player.dashDistance, dashLimitLayerMask))
                player.dashRemainingDistance = hit.distance;
            else
                player.dashRemainingDistance = player.dashDistance;
            return true;
        }
        return false;
    }
}
