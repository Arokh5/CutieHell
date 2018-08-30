using UnityEngine;

public class CameraReferencedMotion : ScriptedAnimation
{
    #region Fields
    [SerializeField]
    private Transform reference;
    [SerializeField]
    [Tooltip("The target position in the reference's local space")]
    private Vector3 localTargetPosition;
    [SerializeField]
    [Tooltip("The target rotation in the reference's local space")]
    private Vector3 localTargetRotation;
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
        UnityEngine.Assertions.Assert.IsNotNull(reference, "ERROR: Reference (Transform) not assigned for CameraReferencedMotion script in GameObject " + gameObject.name);
        gameCamera = Camera.main;
        targetRotationQuat = Quaternion.Euler(localTargetRotation);
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
            if (!reference)
            {
                previewCamera = false;
                Debug.LogError("ERROR: Reference (Transform) not assigned for CameraReferencedMotion script!");
                return;
            }

            if (!initialized)
            {
                initialized = true;
                restored = false;
                Debug.Log("INFO: Remember to unclick the 'Preview Camera' checkbox before moving away from the GameObject + '" + gameObject.name + "'.");
                cameraTransform = Camera.main.transform;
                originalCameraPos = cameraTransform.position;
                originalCameraRot = cameraTransform.rotation;
            }
            cameraTransform.position = reference.TransformPoint(localTargetPosition);
            cameraTransform.rotation = reference.transform.rotation * Quaternion.Euler(localTargetRotation);
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
        startPosition = reference.InverseTransformPoint(gameCamera.transform.position);
        startRotation = Quaternion.Inverse(reference.transform.rotation) * gameCamera.transform.rotation;
    }
    #endregion

    #region Private Methods
    private void MotionAnimation()
    {
        elapsedTime += Time.deltaTime;
        float u = elapsedTime / motionDuration;
        if (u > 1.0f)
            u = 1.0f;

        gameCamera.transform.position = reference.TransformPoint(Vector3.Lerp(startPosition, localTargetPosition, u));
        gameCamera.transform.rotation = reference.transform.rotation * Quaternion.Lerp(startRotation, targetRotationQuat, u);

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