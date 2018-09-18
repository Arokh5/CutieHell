using UnityEngine;

public class SlimeWalkSpeedRandomizer : MonoBehaviour
{
    #region Fields
    public float minValue = 0.8f;
    public float maxValue = 1.1f;
    public ParticleSystem footStepsVFX;
    public AudioClip footStepSFX;
    public AudioSource audioSource;

    private Animator animator;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        animator = GetComponent<Animator>();
        UnityEngine.Assertions.Assert.IsNotNull(animator, "ERROR: An Animator Component could not be found by SlimeWalkSpeedRandomizer in GameObject " + gameObject.name);
    }

    private void OnEnable()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
        float speed = Random.Range(minValue, maxValue);
        animator.SetFloat("Speed", speed);
    }

    public void InstantiateVFX()
    {
        ParticlesManager.instance.LaunchParticleSystem(footStepsVFX, this.transform.position + Vector3.up * 0.1f, footStepsVFX.transform.rotation);
        SoundManager.instance.PlaySfxClip(audioSource, footStepSFX, true);
    }
    #endregion
}
