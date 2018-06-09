using UnityEngine;

public class MovingSoundEmitter : SoundEmitter
{
    [System.Serializable]
    public class MotionInfo
    {
        public Vector3 startOffset;
        public Vector3 endOffset;
        [Tooltip("Set to true in order to use the offset in the local coordinate space of the reference transform.")]
        public bool offsetInLocalSpace;
        public float motionDuration;
    }

    #region Fields
    [Header("Moving Source Setup")]
    public Transform reference;
    public MotionInfo motionInfo;

    private float elapsedTime;
    private Vector3 globalStartPosition;
    private Vector3 globalEndPosition;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(reference, "ERROR: A Transform (reference) has NOT been assigned in MovingSoundEmitter in GameObject '" + gameObject.name + "'!");
        enabled = false;
    }

    private void Update()
    {
        /* Note that this Component get enabled or disabled to trigger its functionality */
        elapsedTime += Time.deltaTime;
        UpdateAudioSourcePosition();
        if (elapsedTime > motionInfo.motionDuration)
            StopSoundEffect();
    }
    #endregion

    #region Protected Methods
    protected override void PlaySoundEffect()
    {
        enabled = true;
        if (!audioSource.isPlaying)
        {
            StartUpEffectInfo();
            audioSource.Play();
        }
    }

    protected override void StopSoundEffect()
    {
        enabled = false;
        audioSource.Stop();
    }
    #endregion

    #region Private Methods
    private void StartUpEffectInfo()
    {
        elapsedTime = 0;
        if (motionInfo.offsetInLocalSpace)
        {
            globalStartPosition = reference.position + reference.TransformDirection(motionInfo.startOffset);
            globalEndPosition = reference.position + reference.TransformDirection(motionInfo.endOffset);
        }
        else
        {
            globalStartPosition = reference.position + motionInfo.startOffset;
            globalEndPosition = reference.position + motionInfo.endOffset;
        }
        UpdateAudioSourcePosition();
    }

    private void UpdateAudioSourcePosition()
    {
        float u = Mathf.Clamp01(elapsedTime / motionInfo.motionDuration);
        Vector3 currentPos = (1 - u) * globalStartPosition + u * globalEndPosition;
        audioSource.transform.position = currentPos;
    }
    #endregion
}
