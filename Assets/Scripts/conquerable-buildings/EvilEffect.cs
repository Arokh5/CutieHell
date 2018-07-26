using System.Collections.Generic;
using UnityEngine;

public class EvilEffect : MonoBehaviour, ITextureChanger
{
    [System.Serializable]
    private struct DamageEffect
    {
        public float evilRadius;
        [Range(0.0f, 1.0f)]
        public float normalizedDamage;
    }

    #region Fields
    [Header("Area of Effect")]
    [ShowOnly]
    [SerializeField]
    private float currentEvilRadius = 0.0f;
    [Tooltip("The normalized radius from which the Main and Alternate textures are blended together.")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float blendStartRadius = 0.8f;
    [Tooltip("The max radius where the Main texture is shown (some of the outer-most section might be blended).")]
    [SerializeField]
    private DamageEffect maxEffect;
    [Tooltip("the min radius where the Main texture is shown right before the Monument is conquered.")]
    [SerializeField]
    private DamageEffect minEffect;

    private float normalizedDamage = 0.0f;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        currentEvilRadius = maxEffect.evilRadius;
    }

    private void OnValidate()
    {
        if (maxEffect.evilRadius < 0)
            maxEffect.evilRadius = 0;

        if (minEffect.evilRadius < 0)
            minEffect.evilRadius = 0;

        if (minEffect.evilRadius > maxEffect.evilRadius)
            minEffect.evilRadius = maxEffect.evilRadius;

        if (maxEffect.normalizedDamage > minEffect.normalizedDamage)
            maxEffect.normalizedDamage = minEffect.normalizedDamage;

        if (blendStartRadius * maxEffect.evilRadius < minEffect.evilRadius)
            blendStartRadius = minEffect.evilRadius / maxEffect.evilRadius;

        SetNormalizedMonumentDamage(normalizedDamage);
    }

    private void OnDrawGizmosSelected()
    {
        // Min radius
        Gizmos.color = new Color(0, 0, 0, 1.0f);
        Gizmos.DrawWireSphere(transform.position, minEffect.evilRadius);
        // Blend start radius
        Gizmos.color = new Color(0.55f, 0.27f, 0.07f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, blendStartRadius * maxEffect.evilRadius);
        // Max radius (cute texture start)
        Gizmos.color = new Color(0, 1, 0, 1.0f);
        Gizmos.DrawWireSphere(transform.position, maxEffect.evilRadius);
    }
    #endregion

    #region Public Methods
    public void SetNormalizedMonumentDamage(float normalizedDamage)
    {
        this.normalizedDamage = normalizedDamage;

        if (normalizedDamage <= maxEffect.normalizedDamage)
            currentEvilRadius = maxEffect.evilRadius;
        else if (normalizedDamage > minEffect.normalizedDamage)
            currentEvilRadius = minEffect.evilRadius;
        else
        {
            float adjustedDamage = (normalizedDamage - maxEffect.normalizedDamage) / (minEffect.normalizedDamage - maxEffect.normalizedDamage);
            currentEvilRadius = minEffect.evilRadius + (1 - adjustedDamage) * (maxEffect.evilRadius - minEffect.evilRadius);
        }
    }

    // ITextureChanger
    public float GetNormalizedBlendStartRadius()
    {
        return blendStartRadius;
    }

    // ITextureChanger
    public float GetEffectMaxRadius()
    {
        return currentEvilRadius;
    }
    #endregion
}
