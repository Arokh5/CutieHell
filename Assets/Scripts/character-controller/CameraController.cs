using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Transform player;

    private const float xSpeed = 250f;
    private const float ySpeed = 120f;
    private const float yMinLimit = -20f;
    private const float yMaxLimit = 80f;
    private const int zoomRate = 50;
    private const float lerpSpeed = 0.5f;

    private float distance;
    private float x;
    private float y;
    private float t;

    private void Awake()
    {
        distance = 3.5f;
        x = 0f;
        y = 0f;
        t = 0f;
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
        float test = 0;

        if(InputManager.instance.GetRightStickLeft() || InputManager.instance.GetRightStickRight())
            x += xSpeed * 0.01f * InputManager.instance.GetRightStickLeftValue();
        if(InputManager.instance.GetRightStickUp() || InputManager.instance.GetRightStickDown())
            y += ySpeed * 0.01f * InputManager.instance.GetRightStickUpValue();

        test = y;

        if (distance < 2.5f)
        {
            distance = 2.5f;
        }
        if (distance > 20.0f)
        {
            distance = 20.0f;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        if (y == yMinLimit || test == yMinLimit)
        {
            // This is to allow the camera to slide across the bottom if the player is too low in the y
            //distance += -(InputManager.instance.GetRightStickUpValue() * Time.deltaTime) * 10 * Mathf.Abs(distance);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 2.0f, -distance) + player.position;

        transform.rotation = rotation;
        transform.position = position;

        SetPlayerDirection(LerpRotation(rotation.eulerAngles.y));
    }

    private float LerpRotation(float cameraRotationY)
    {
        float playerRotationY = player.rotation.eulerAngles.y;

        if (cameraRotationY != playerRotationY)
        {
            // Increate the t interpolater
            t += lerpSpeed * Time.deltaTime;

            playerRotationY = Mathf.LerpAngle(playerRotationY, cameraRotationY, t);

            if (playerRotationY == cameraRotationY)
            {
                t = 0f;
            }
        }

        return playerRotationY;
    }

    private void SetPlayerDirection(float rotation)
    {
        if (!Input.GetMouseButton(0))
        {
            // Set player rotation to define his movement direction
            player.rotation = Quaternion.Euler(player.rotation.x, rotation, player.rotation.z);
        }
        else
        {
            t = 0;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}