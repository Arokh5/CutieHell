using UnityEngine;

public class MovingSoundEmitter : SoundEmitter
{
    [System.Serializable]
    public class MotionInfo
    {
        public Vector3 startOffset;
        public Vector3 endOffset;
        public bool offsetInReferenceLocalSpace;
        public float motionDuration;
    }

    #region Fields
    public bool restartOnRetrigger;
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
        if (restartOnRetrigger || !audioSource.isPlaying)
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
        if (motionInfo.offsetInReferenceLocalSpace)
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
