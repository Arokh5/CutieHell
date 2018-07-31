using System.Collections.Generic;
using UnityEngine;

public class BuildingEffects : MonoBehaviour, ITextureChanger
{
    #region Fields
    private Building attachedBuilding;

    [Header("Elements setup")]
    [SerializeField]
    private GameObject buildingRenderer;
    [SerializeField]
    private GameObject alternateBuildingRenderer;

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
    private float maxEvilRadius = 35.0f;
    [Tooltip("the min radius where the Main texture is shown right before the Monument is conquered.")]
    [SerializeField]
    private float minEvilRadius = 5.0f;
    [SerializeField]
    private List<Convertible> convertibles;
    [Tooltip("This list will be automatically filled with all EvilEffects that are children of this GameObject")]
    [ShowOnly]
    public List<EvilEffect> evilEffects = new List<EvilEffect>();

    [Header("Effects timing")]
    [Tooltip("The duration (in seconds) that the dark to cute conversion takes.")]
    [SerializeField]
    private float conquerEffectDuration = 1;


    private float conquerEffectElapsedTime = 0;
    private bool conquering = false;
    private bool conquered = false;
    private bool convertionsTriggered = false;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        attachedBuilding = GetComponent<Building>();
        UnityEngine.Assertions.Assert.IsNotNull(attachedBuilding, "ERROR: A Building Component could not be found by BuildingEffects in GameObject " + gameObject.name);

        UnityEngine.Assertions.Assert.IsNotNull(buildingRenderer, "ERROR: Building Renderer not assigned for BuildingEffects in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateBuildingRenderer, "ERROR: Alternate Building Renderer not assigned for BuildingEffects script in GameObject " + gameObject.name);
        buildingRenderer.gameObject.SetActive(true);
        alternateBuildingRenderer.gameObject.SetActive(false);

        GetComponentsInChildren(true, evilEffects);
    }

    private void Start()
    {
        alternateBuildingRenderer.transform.localScale = Vector3.zero;
        currentEvilRadius = maxEvilRadius;
        conquered = false;
    }

    private void Update()
    {
        if (conquering)
        {
            conquerEffectElapsedTime += Time.deltaTime;
            if (conquerEffectElapsedTime < conquerEffectDuration)
            {
                ConquerEffect();
            }
            else
            {
                conquerEffectElapsedTime = conquerEffectDuration;
                ConquerEffect();
                conquering = false;
                conquered = true;
                attachedBuilding.BuildingConverted();
                attachedBuilding.animating = false;
            }
        }
    }

#if UNITY_EDITOR
    private TextureChangerSource tcs = null;
#endif
    private void OnValidate()
    {
        if (maxEvilRadius < 0)
            maxEvilRadius = 0;

        if (minEvilRadius < 0)
            minEvilRadius = 0;

        if (minEvilRadius > maxEvilRadius)
            minEvilRadius = maxEvilRadius;

        if (blendStartRadius * maxEvilRadius < minEvilRadius)
            blendStartRadius = minEvilRadius / maxEvilRadius;

        if (!conquering)
            currentEvilRadius = maxEvilRadius;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (tcs == null)
                tcs = FindObjectOfType<TextureChangerSource>();

            tcs.ITextureChangerUpdate();
        }
#endif
    }

    private void OnDrawGizmosSelected()
    {
        // Min radius
        Gizmos.color = new Color(0, 0, 0, 1.0f);
        Gizmos.DrawWireSphere(transform.position, minEvilRadius);
        // Blend start radius
        Gizmos.color = new Color(0.55f, 0.27f, 0.07f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, blendStartRadius * maxEvilRadius);
        // Max radius (cute texture start)
        Gizmos.color = new Color(0, 1, 0, 1.0f);
        Gizmos.DrawWireSphere(transform.position, maxEvilRadius);
    }
    #endregion

    #region Public Methods
    public void StartConquerEffect()
    {
        if (!conquering)
        {
            attachedBuilding.animating = true;
            conquering = true;
            conquerEffectElapsedTime = 0;
            alternateBuildingRenderer.transform.localScale = Vector3.zero;
            alternateBuildingRenderer.gameObject.SetActive(true);
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

    public void SetBuildingConquerProgress(float normalizedConquerProgress)
    {
        //buildingRenderer.material.SetFloat("_ConquerFactor", conquerFactor);
        normalizedConquerProgress = Mathf.Clamp01(normalizedConquerProgress);
        currentEvilRadius = minEvilRadius + (1 - normalizedConquerProgress) * (maxEvilRadius - minEvilRadius);
        foreach (EvilEffect evilEffect in evilEffects)
        {
            evilEffect.SetNormalizedMonumentDamage(normalizedConquerProgress);
        }
    }
    #endregion

    #region Private Methods
    private void ConquerEffect()
    {
        float progress = conquerEffectElapsedTime / conquerEffectDuration;
        currentEvilRadius = (1 - progress) * minEvilRadius;

        float tipingPoint = 0.5f;

        if (progress < tipingPoint)
        {
            buildingRenderer.transform.localScale = (1 - 2 * progress) * Vector3.one;
        }
        else
        {
            TriggerConvertions();
            // Rescale the progress to fall in the range [0,1]
            progress = (progress - tipingPoint) / (1 - tipingPoint);
            alternateBuildingRenderer.transform.localScale = progress * Vector3.one;
        }
    }

    private void TriggerConvertions()
    {
        if (!convertionsTriggered)
        {
            convertionsTriggered = true;
            foreach (Convertible convertible in convertibles)
                convertible.Convert();
        }
    }
    #endregion
}
