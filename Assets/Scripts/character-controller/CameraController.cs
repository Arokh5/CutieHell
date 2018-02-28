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

    private float distance;
    private float x;
    private float y;

    private void Awake()
    {
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
        Vector3 position = rotation * new Vector3(0.5f, 1.75f, -distance) + player.position;

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
}