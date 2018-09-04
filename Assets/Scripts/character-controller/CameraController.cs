using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private LayerMask viewPosCheckLayerMask;

    private Transform player;
    private Player playerScript;
    private CapsuleCollider playerCapsuleCollider;

    private const float xSpeed = 4.20f;
    private const float ySpeed = 1.2f;
    private const float yMinLimit = -20f;
    private const float yMaxLimit = 40f;
    private const float lerpSpeed = 0.1f;
    private const float transitionTime = 0.31f;
    private float timeOnTransition = 10.0f;
    private Player.CameraState lastState;
    [HideInInspector]
    public float timeSinceLastAction = 0.0f;
    [HideInInspector]
    public bool slowAction, fastAction;

    private int collisionLayers;
    [ShowOnly]
    public float x;
    [ShowOnly]
    public float y;

    public float aR, aF, aU,zX,zY,zZ; 

    /* Player camera values */
    public float distance;
    public float cameraX;
    public float cameraY;
    public float focusDistance;
    public float focusX;
    public float focusY;
    public float fov;

    //private float initialDistance;
    [HideInInspector]
    public Vector3 cameraPositionOnConeAttack;

    [Header("Strong Attack setup")]
    public float strongAttackDistance;
    public float yStrongAttackMin;
    public float yStrongAttackMax;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        x = 0f;
        y = 0f;
        slowAction = fastAction = false;
        player = GameManager.instance.GetPlayer1().transform;
        playerScript = player.GetComponent<Player>();
        playerCapsuleCollider = player.GetComponent<CapsuleCollider>();

        zZ = distance = 3.7f;
        zX = cameraX = 0.45f;
        zY = cameraY = 2.1f;
        aF = focusDistance = 0.65f;
        aR = focusX = 0.55f;
        aU = focusY = 1.8f;
        fov = 60f;

        aR = 0.35f;
        aF = 1.7f;
        aU = 2.0f;
        zX = 1.1f;
        zY = 0.13f;
        zZ = 4.22f;
    }

    private void Start()
    {
        Vector2 angles = transform.eulerAngles;
        x = 35.5f;
        y = 0.0f;
        lastState = playerScript.cameraState;
    }

    private void Update()
    {
        RotateCamera();
    }
    #endregion

    #region Public Methods
    public void SetCameraXAngle(float x)
    {
        this.x = x;
    }

    public void SetCameraYAngle(float y)
    {
        this.y = y;
    }
    #endregion

    #region Private Methods
    private void RotateCamera()
    {
        if(!GameManager.instance.gameIsPaused)
        {
            if (playerScript.cameraState != Player.CameraState.STILL)
            {
                if (InputManager.instance.GetRightStickLeft() || InputManager.instance.GetRightStickRight())
                    x += xSpeed * InputManager.instance.GetRightStickHorizontalSqrValue();
                if (InputManager.instance.GetRightStickUp() || InputManager.instance.GetRightStickDown())
                    y += ySpeed * InputManager.instance.GetRightStickVerticalSqrValue();
            }

            if (playerScript.cameraState != lastState)
            {
                timeOnTransition = 0.0f;
            }
            switch (playerScript.cameraState)
            {
                case Player.CameraState.STILL:
                    break;
                case Player.CameraState.MOVE:
                    {
                        y = ClampAngle(y, yMinLimit, yMaxLimit);
                        Quaternion rotation = Quaternion.Euler(y, x, 0);
                        float noCollisionDistance = distance;

                        for (float zOffset = distance; zOffset >= 0.5f; zOffset -= 0.025f)
                        {
                            noCollisionDistance = zOffset;
                            Vector3 tempPos = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;

                            if (DoubleViewingPosCheck(tempPos, zOffset))
                            {
                                break;
                            }
                        }
                        if (timeOnTransition < transitionTime)
                        {
                            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fov, 0.1f);
                            timeOnTransition += Time.deltaTime;
                            this.transform.position = Vector3.Lerp(this.transform.position, rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position, timeOnTransition);
                        }
                        else
                        {
                            Camera.main.fieldOfView = fov;
                            Vector3 position = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;
                            this.transform.position = position;
                        }
                        if (timeSinceLastAction < 0.5f)
                        {
                            timeSinceLastAction += Time.deltaTime;
                            if (fastAction)
                            {
                                SetPlayerDirection(rotation.eulerAngles.y, 0.7f);
                            }
                            else if(slowAction)
                            {
                                SetPlayerDirection(rotation.eulerAngles.y, 0.2f);
                            }
                            else
                            {
                                SetPlayerDirection(rotation.eulerAngles.y);//, playerScript.rb.velocity.magnitude / 10.0f);
                            }
                        }
                        else
                        {
                            fastAction = slowAction = false;
                        }
                        this.transform.LookAt(player.transform.position + rotation * Vector3.up * focusY + rotation * Vector3.right * focusX + rotation * Vector3.forward * focusDistance);
                    }

                    break;
                case Player.CameraState.STRONG_ATTACK:
                    {
                        y = ClampAngle(y, yStrongAttackMin, yStrongAttackMax);                        

                        Quaternion rotation = Quaternion.Euler(y, x, 0);
                        float noCollisionDistance = strongAttackDistance;

                        if (timeOnTransition < transitionTime)
                        {
                            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fov, 0.1f);
                            timeOnTransition += Time.deltaTime;
                            this.transform.position = Vector3.Lerp(this.transform.position, rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position, timeOnTransition / 2f);
                        }
                        else
                        {
                            Camera.main.fieldOfView = fov;
                            this.transform.position = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;
                        }
                        SetPlayerDirection(rotation.eulerAngles.y, 0.7f);                    
                        this.transform.LookAt(player.transform.position + rotation * Vector3.up * focusY + rotation * Vector3.right * focusX + rotation * Vector3.forward * focusDistance);
                    }
                    break;
                case Player.CameraState.ZOOMOUT:
                    this.transform.Translate((this.transform.forward * -4 + this.transform.up * 2) * Time.deltaTime, Space.World);
                    break;
                case Player.CameraState.ZOOMIN:
                    {
                        Quaternion rotation = this.transform.rotation;
                        float noCollisionDistance = distance;
                        timeOnTransition += Time.deltaTime;
                        for (float zOffset = distance; zOffset >= 0.5f; zOffset -= 0.025f)
                        {
                            noCollisionDistance = zOffset;
                            Vector3 tempPos = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;

                            if (DoubleViewingPosCheck(tempPos, zOffset))
                            {
                                break;
                            }
                        }
                        this.transform.position = Vector3.Lerp(this.transform.position, rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position, 4.0f);
                        this.transform.LookAt(player.transform.position + player.transform.up * focusY + player.transform.right * focusX + player.transform.forward * focusDistance);
                        break;
                    }
                case Player.CameraState.TRANSITION:
                    {
                        y = ClampAngle(y, yMinLimit, yMaxLimit);
                        Quaternion rotation = Quaternion.Euler(y, x, 0);
                        float noCollisionDistance = distance;
                        timeOnTransition += Time.deltaTime;
                        this.transform.position = Vector3.Lerp(this.transform.position, rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position, timeOnTransition / 2f);
                        this.transform.LookAt(player.transform.position + rotation * Vector3.up * focusY + rotation * Vector3.right * focusX + rotation * Vector3.forward * focusDistance);
                        break;
                    }
                case Player.CameraState.METEORITEAIM:
                    {
                        y = ClampAngle(y, 50, 75);
                        x = ClampAngle(x, -45, 45);
                        Quaternion rotation = Quaternion.Euler(y, x, 0);

                        Vector3 position = rotation * new Vector3(zX, zY, -zZ) + player.position;
                        this.transform.position = position;

                        SetPlayerDirection(rotation.eulerAngles.y);
                        if (Input.GetKey(KeyCode.A))
                        {
                            aR += Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.S))
                        {
                            aR -= Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            aU += Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.F))
                        {
                            aU -= Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.G))
                        {
                            aF += Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.H))
                        {
                            aF -= Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.Q))
                        {
                            zX += Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.W))
                        {
                            zX -= Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.E))
                        {
                            zY += Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.R))
                        {
                            zY -= Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.T))
                        {
                            zZ += Time.deltaTime;
                        }
                        if (Input.GetKey(KeyCode.Y))
                        {
                            zZ -= Time.deltaTime;
                        }
                        this.transform.LookAt(player.transform.position + rotation * Vector3.up * aU + rotation * Vector3.right * aR + rotation * Vector3.forward * aF);
                    }           
                    break;
                case Player.CameraState.CONEATTACK:
                    {
                    y = ClampAngle(y, 15, 20);
                    Quaternion rotation = Quaternion.Euler(y, x, 0);
                    float noCollisionDistance = distance + 6;

                    for (float zOffset = distance + 6; zOffset >= 0.5f; zOffset -= 0.025f)
                    {
                        noCollisionDistance = zOffset;
                        Vector3 tempPos = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;

                        if (DoubleViewingPosCheck(tempPos, zOffset))
                        {
                            break;
                        }
                    }

                    timeOnTransition += Time.deltaTime;
                    this.transform.position = Vector3.Lerp(this.transform.position, rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position, 0.15f);

                    if (timeSinceLastAction < 0.5f)
                    {
                        timeSinceLastAction += Time.deltaTime;
                        if (fastAction)
                        {
                            SetPlayerDirection(rotation.eulerAngles.y, 0.7f);
                        }
                        else if (slowAction)
                        {
                            SetPlayerDirection(rotation.eulerAngles.y, 0.2f);
                        }
                        else
                        {
                            SetPlayerDirection(rotation.eulerAngles.y);//, playerScript.rb.velocity.magnitude / 10.0f);
                        }
                    }
                    else
                    {
                        fastAction = slowAction = false;
                    }
                    this.transform.LookAt(player.transform.position + rotation * Vector3.up * focusY + rotation * Vector3.right * focusX + rotation * Vector3.forward * focusDistance);
                }
                    break;
                case Player.CameraState.DASH:
                    { 
                    y = ClampAngle(y, 20, 22);
                    Quaternion rotation = Quaternion.Euler(y, x, 0);
                    float noCollisionDistance = distance + 4;

                    for (float zOffset = distance + 6; zOffset >= 0.5f; zOffset -= 0.025f)
                    {
                        noCollisionDistance = zOffset;
                        Vector3 tempPos = rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position;

                        if (DoubleViewingPosCheck(tempPos, zOffset))
                        {
                            break;
                        }
                    }

                    timeOnTransition += Time.deltaTime;
                    this.transform.position = Vector3.Lerp(this.transform.position, rotation * new Vector3(cameraX, cameraY, -noCollisionDistance) + player.position, 0.25f);

                    if (timeSinceLastAction < 0.5f)
                    {
                        timeSinceLastAction += Time.deltaTime;
                        if (fastAction)
                        {
                            SetPlayerDirection(rotation.eulerAngles.y, 0.7f);
                        }
                        else if (slowAction)
                        {
                            SetPlayerDirection(rotation.eulerAngles.y, 0.2f);
                        }
                        else
                        {
                            SetPlayerDirection(rotation.eulerAngles.y);//, playerScript.rb.velocity.magnitude / 10.0f);
                        }
                    }
                    else
                    {
                        fastAction = slowAction = false;
                    }
                    this.transform.LookAt(player.transform.position + rotation * Vector3.up * focusY + rotation * Vector3.right * focusX + rotation * Vector3.forward * focusDistance);
                }
                break;

                default:
                    break;
            }
            lastState = playerScript.cameraState;
        }
    }
    //set player lerp camera, get the higher

    private void SetPlayerDirection(float rotation, float lerp = 0.8f)
    {
        player.rotation = Quaternion.LerpUnclamped(player.rotation ,Quaternion.Euler(player.rotation.x, rotation, player.rotation.z), lerp);
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
        float playerFocusHeight = playerCapsuleCollider.height * 0.5f;
        return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight, offset);
    }

    private bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight) 
    {
        RaycastHit hit;

        if (Physics.Raycast(checkPos, player.position + (Vector3.up * deltaPlayerHeight) - checkPos, out hit, viewPosCheckLayerMask)) 
        {
            if (Helpers.GameObjectInLayerMask(hit.transform.gameObject, viewPosCheckLayerMask))
            {
                return false;
            }
        }
        return true;
    }

    private bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float offset) {
        RaycastHit hit;
        if (Physics.Raycast(player.position + (Vector3.up * deltaPlayerHeight), checkPos - player.position - (Vector3.up * deltaPlayerHeight), out hit, offset, viewPosCheckLayerMask)) 
        {

            if (Helpers.GameObjectInLayerMask(hit.transform.gameObject, viewPosCheckLayerMask))
            {
                return false;
            }
        }
        return true;
    }
    #endregion
}
