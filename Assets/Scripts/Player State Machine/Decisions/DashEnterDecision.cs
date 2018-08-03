using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Decisions/DashEnterDecision")]
public class DashEnterDecision : Decision
{
    public float limitCheckUpwardsOffset = 0.2f;
    public LayerMask dashLimitLayerMask;

    public override bool Decide(Player player)
    {
        if (!player.knockbackActive && InputManager.instance.GetL2ButtonDown())
        {
            float threshold = InputManager.joystickThreshold;
            float verticalValue = -InputManager.instance.GetLeftStickVerticalValue();
            if (verticalValue > -threshold && verticalValue < threshold)
                verticalValue = 0.0f;
            float horizontalValue = InputManager.instance.GetLeftStickHorizontalValue();
            if (horizontalValue > -threshold && horizontalValue < threshold)
                horizontalValue = 0.0f;

            Vector3 direction;
            if (horizontalValue != 0.0f || verticalValue != 0.0f)
            {
                direction = new Vector3(horizontalValue, 0, verticalValue);
                direction = player.mainCameraController.transform.TransformDirection(direction);
            }
            else
            {
                direction = player.mainCameraController.transform.forward;
            }

            direction.y = 0.0f;
            direction.Normalize();

            player.dashDirection = direction;
            player.dashElapsedTime = 0.0f;

            // Calculate limit distance
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position + limitCheckUpwardsOffset * Vector3.up, player.dashDirection, out hit, player.dashDistance, dashLimitLayerMask))
                player.dashRemainingDistance = hit.distance;
            else
                player.dashRemainingDistance = player.dashDistance;

            return true;
        }
        return false;
    }
}
