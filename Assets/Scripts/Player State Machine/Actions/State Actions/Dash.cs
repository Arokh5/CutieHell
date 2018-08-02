using UnityEngine;

[CreateAssetMenu(menuName = "Player State Machine/Actions/Dash")]
public class Dash : StateAction
{
    public LayerMask walkableLayer;

    public override void Act(Player player)
    {
        Vector3 playerPos = player.rb.position;

        float speed = player.dashDistance / player.dashDuration;

        float u = player.dashElapsedTime / player.dashDuration;
        float easedSpeed = speed * (1 - u) * 2;

        playerPos += player.dashDirection * easedSpeed * Time.deltaTime;
        CheckGround(player, ref playerPos);
        player.rb.position = playerPos;

        player.dashElapsedTime += Time.deltaTime;
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
        else
        {
            playerPos = player.lastValidPosition;
        }
    }
}
