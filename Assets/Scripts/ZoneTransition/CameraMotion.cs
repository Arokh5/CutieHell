using UnityEngine;

public class CameraMotion : ScriptedAnimation
{
    #region Fields
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private Vector3 targetRotation;
    [SerializeField]
    private float motionDuration = 1.0f;

    private bool animating;
    private Camera gameCamera;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Quaternion targetRotationQuat;
    private float elapsedTime;

#if UNITY_EDITOR
    [SerializeField]
    private bool previewCamera = false;
    private bool initialized = false;
    private bool restored = true;
    private Transform cameraTransform;
    private Vector3 originalCameraPos;
    private Quaternion originalCameraRot;
#endif

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        gameCamera = Camera.main;
        targetRotationQuat = Quaternion.Euler(targetRotation);
    }

    private void Update()
    {
        if (animating)
        {
            MotionAnimation();
        }
    }

    private void OnValidate()
    {
        if (motionDuration < 0.0f)
            motionDuration = 0.0f;

#if UNITY_EDITOR
        if (previewCamera)
        {
            if (!initialized)
            {
                initialized = true;
                restored = false;
                Debug.Log("INFO: Remember to unclick the 'Preview Camera' checkbox before moving away from the GameObject + '" + gameObject.name + "'.");
                cameraTransform = Camera.main.transform;
                originalCameraPos = cameraTransform.position;
                originalCameraRot = cameraTransform.rotation;
            }
            cameraTransform.position = targetPosition;
            cameraTransform.rotation = Quaternion.Euler(targetRotation);
        }
        else if (!restored)
        {
            restored = true;
            initialized = false;
            cameraTransform.position = originalCameraPos;
            cameraTransform.rotation = originalCameraRot;
        }
#endif
    }
    #endregion

    #region Protected Methods
    // CameraAnimation
    protected override void StartAnimationInternal()
    {
        animating = true;
        elapsedTime = 0.0f;
        startPosition = gameCamera.transform.position;
        startRotation = gameCamera.transform.rotation;
    }
    #endregion

    #region
    private void MotionAnimation()
    {
        elapsedTime += Time.deltaTime;
        float u = elapsedTime / motionDuration;
        if (u > 1.0f)
            u = 1.0f;

        gameCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, u);
        gameCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotationQuat, u);

        if (u >= 1.0f)
        {
            EndAnimation();
        }
    }

    private void EndAnimation()
    {
        animating = false;
        OnAnimationFinished();
    }
    #endregion
}
