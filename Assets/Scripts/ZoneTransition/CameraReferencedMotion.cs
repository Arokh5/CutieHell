using UnityEngine;

public class CameraReferencedMotion : ScriptedAnimation
{
    #region Fields
    [SerializeField]
    private bool hideObstructions = false;
    [SerializeField]
    private float yOffset = 0.0f;

    private Renderer[] hiddenRenderers;

    [Header("Camera setup")]
    [SerializeField]
    private bool allowRuntimeReferenceSet = false;
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

    [Header("Animation triggering")]
    [SerializeField]
    [Tooltip("Defines if this ScriptedAnimation will attempt to find an Animator in its reference and set the trigger specified by triggerName.")]
    private bool triggerAnimation;
    [SerializeField]
    [Tooltip("Defines the timing og the animation is triggerd. If ticked, the animation is triggered at the end of the ScriptedAnimation. Otherwise, it is triggered an the beginning.")]
    private bool triggerOnEnd;
    [SerializeField]
    private string triggerName;

    private Animator referenceAnimator;

#if UNITY_EDITOR
    [Header("Editor tools")]
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
        if (!allowRuntimeReferenceSet)
        {
            UnityEngine.Assertions.Assert.IsNotNull(reference, "ERROR: Reference (Transform) not assigned for CameraReferencedMotion script in GameObject '" + gameObject.name + "'!");
        }
        gameCamera = Camera.main;
        targetRotationQuat = Quaternion.Euler(localTargetRotation);
        ValidateAnimatorParameter();
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

    #region Public Methods
    public void SetReference(Transform reference)
    {
        this.reference = reference;
        ValidateAnimatorParameter();
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

        if (hideObstructions)
        {
            Vector3 cameraFinalPos = reference.TransformPoint(localTargetPosition);
            // Hide obstructions to face
            obstructionsHandler.HideObstructions(reference.transform.position + yOffset * Vector3.up, cameraFinalPos);
            // Hide obstructions to feet
            obstructionsHandler.HideObstructions(reference.transform.position + 0.2f * Vector3.up, cameraFinalPos);
        }

        if (triggerAnimation && !triggerOnEnd)
        {
            referenceAnimator.SetTrigger(triggerName);
        }
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

        if (triggerAnimation && triggerOnEnd)
        {
            referenceAnimator.SetTrigger(triggerName);
        }

        OnAnimationFinished();
    }

    private void ValidateAnimatorParameter()
    {
        if (triggerAnimation)
        {
            referenceAnimator = reference.GetComponent<Animator>();
            UnityEngine.Assertions.Assert.IsNotNull(referenceAnimator, "ERROR: The Reference (Transform) assigned for CameraReferencedMotion script in GameObject " + gameObject.name + "' does not have an Animator Component attached to it!");
            bool found = false;
            foreach (AnimatorControllerParameter acp in referenceAnimator.parameters)
            {
                if (acp.name == triggerName)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Debug.LogError("ERROR: The Animator found in the Reference (Transform) assigned for CameraReferencedMotion script in GameObject " + gameObject.name + "' does NOT have an animator parameter called '" + triggerName + "'!");
            }
        }
    }
    #endregion
}
