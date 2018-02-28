using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Transform player;
    [SerializeField]
    private bool lookAt = false;


    private const float xSpeed = 2.5f;
    private const float ySpeed = 1.2f;
    private const float yMinLimit = -20f;
    private const float yMaxLimit = 40f;
    private const float lerpSpeed = 0.1f;

    private int collisionLayers;
    private float distance;
    private float x;
    private float y;

    private void Awake()
    {
        collisionLayers = 1 << 4;
        distance = 3.0f;
        x = 0f;
        y = 0f;
    }

    private void Start()
    {
        Vector2 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        if(InputManager.instance.GetRightStickLeft() || InputManager.instance.GetRightStickRight())
            x += xSpeed * InputManager.instance.GetRightStickLeftValue();
        if(InputManager.instance.GetRightStickUp() || InputManager.instance.GetRightStickDown())
            y += ySpeed * InputManager.instance.GetRightStickUpValue();

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        /* Checks for collisions */
        float noCollisionDistance = distance;
        Vector3 baseTempPosition = rotation * new Vector3(0.5f, 1.75f, -noCollisionDistance) + player.position;

        for( float zOffset = distance; zOffset >= 0.5f; zOffset -= 0.1f) 
        {
            noCollisionDistance = zOffset;

            if (DoubleViewingPosCheck(baseTempPosition + rotation * new Vector3(0.5f, 1.75f, -noCollisionDistance), zOffset)) {
                break;
            }
        }

        /* Ends collision detection */

        Vector3 position = rotation * new Vector3(0.5f, 1.75f, -noCollisionDistance) + player.position;

        transform.position = position;

        SetPlayerDirection(rotation.eulerAngles.y);

        this.transform.LookAt(player.transform.position + player.transform.up  * 1.7f + player.transform.right * 0.3f + player.transform.forward * 0.4f);
    }

    private float LerpRotation(float cameraRotationY)
    {
        float playerRotationY = player.rotation.eulerAngles.y;

        if (cameraRotationY != playerRotationY)
        {
            playerRotationY = Mathf.LerpAngle(playerRotationY, cameraRotationY, lerpSpeed);
        }
        return playerRotationY;
    }

    private void SetPlayerDirection(float rotation)
    {
        if (!Input.GetMouseButton(0))
        {
            player.rotation = Quaternion.Euler(player.rotation.x, rotation, player.rotation.z);
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360)
            angle += 360;
        while (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    private bool DoubleViewingPosCheck(Vector3 checkPos, float offset) 
    {
        float playerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.5f;
        return ViewingPosCheck(checkPos, playerFocusHeight);
    }

    // Check for collision from camera to player.
    private bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight) {
        RaycastHit hit;
        // If a raycast from the check position to the player hits something...
        if (Physics.Raycast(checkPos, player.position + (Vector3.up * deltaPlayerHeight) - checkPos, out hit)) {
            // ... if it is not the player...
            //Debug.DrawRay(checkPos, player.position + (Vector3.up * deltaPlayerHeight) - checkPos, Color.green);
            if (hit.transform.gameObject.layer == 4) {
                // This position isn't appropriate.
                return false;
            }
        }
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        return true;
    }

    // Check for collision from player to camera.
    private bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance) {
        RaycastHit hit;

        if (Physics.Raycast(player.position + (Vector3.up * deltaPlayerHeight), checkPos - player.position, out hit)) {
            if (hit.transform != transform && hit.transform.gameObject.layer == collisionLayers) {
                return false;
            }
        }
        return true;
    }
}