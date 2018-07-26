using System.Collections;
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

    [Header("Effects timing")]
    [Tooltip("The duration (in seconds) for which the conquerable object is considered to be \"under attack\" after the last actual attack happened.")]
    [SerializeField]
    private float underAttackStateDuration = 1;
    [Tooltip("The duration (in seconds) that the dark to cute conversion takes.")]
    [SerializeField]
    private float conquerEffectDuration = 1;

    private float underAttackElapsedTime = 0;
    private float conquerEffectElapsedTime = 0;

    private bool underAttack = false;
    private bool conquering = false;
    private bool conquered = false;
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
    }

    private void Start()
    {
        alternateBuildingRenderer.transform.localScale = Vector3.zero;
        currentEvilRadius = maxEvilRadius;
        conquered = false;
    }

    private void Update()
    {
        if (underAttack)
        {
            underAttackElapsedTime += Time.deltaTime;
            if (underAttackElapsedTime >= underAttackStateDuration)
            {
                SetUnderAttack(false);
                underAttackElapsedTime = 0;
            }
        }

        if (conquering)
        {
            conquerEffectElapsedTime += Time.deltaTime;
            if (conquerEffectElapsedTime < conquerEffectDuration)
            {
                ConquerEffect();
            }
            else
            {
                conquering = false;
                conquered = true;
                attachedBuilding.BuildingConverted();
                attachedBuilding.animating = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.75f);
        Gizmos.DrawWireSphere(transform.position, maxEvilRadius);
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawWireSphere(transform.position, blendStartRadius);
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
        if (conquered)
            return blendStartRadius / maxEvilRadius;
        else if (conquering)
            return (conquerEffectElapsedTime / conquerEffectDuration) * (blendStartRadius / maxEvilRadius);
        else
            return 0.0f;
    }

    // ITextureChanger
    public float GetEffectMaxRadius()
    {
        return currentEvilRadius;
    }

    public void SetUnderAttack(bool underAttackState)
    {
        if (underAttackState)
            underAttackElapsedTime = 0;

        if (underAttack != underAttackState)
        {
            if (underAttackState)
            {
                //buildingRenderer.material.SetFloat("_UnderAttack", 1);
            }
            else
            {
                //buildingRenderer.material.SetFloat("_UnderAttack", 0);
            }
        }
        underAttack = underAttackState;
    }

    public void SetBuildingConquerProgress(float normalizedConquerProgress)
    {
        //buildingRenderer.material.SetFloat("_ConquerFactor", conquerFactor);
        normalizedConquerProgress = Mathf.Clamp01(normalizedConquerProgress);
        currentEvilRadius = minEvilRadius + (1 - normalizedConquerProgress) * (maxEvilRadius - minEvilRadius);
    }
    #endregion

    #region Private Methods
    private void ConquerEffect()
    {
        
        float progress = conquerEffectElapsedTime / conquerEffectDuration;
        currentEvilRadius = (1 - progress) * minEvilRadius;

        if (progress < 0.5f)
        {
            buildingRenderer.transform.localScale = (1 - 2 * progress) * Vector3.one;
        }
        else
        {
            // Rescale the progress to fall in the range [0,1]
            progress = (progress - 0.6f) / (1 - 0.6f);
            alternateBuildingRenderer.transform.localScale = progress * Vector3.one;
        }

        // Now we attempt to convert convertible props

        foreach (Convertible convertible in convertibles)
        {
            if (!convertible.IsConverting() && !convertible.IsConverted() && Vector3.Distance(transform.position, convertible.transform.position) < currentEvilRadius)
            {
                convertible.Convert();
            }
        }
    }
    #endregion
}
