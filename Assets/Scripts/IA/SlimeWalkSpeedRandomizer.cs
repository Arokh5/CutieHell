using UnityEngine;

public class SlimeWalkSpeedRandomizer : MonoBehaviour
{
    #region Fields
    public float minValue = 0.8f;
    public float maxValue = 1.1f;

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
    #endregion
}
