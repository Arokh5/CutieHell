using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEffects : MonoBehaviour
{
    #region Fields
    private Building attachedBuilding;

    [Header("Elements setup")]
    [SerializeField]
    private MeshRenderer buildingRenderer;
    [SerializeField]
    private MeshRenderer alternateBuildingRenderer;

    [Header("Area of Effect")]
    public float effectOnMapRadius = 0.0f;
    [Tooltip("The max radius within which the texture will be completely changed while the building is being damaged.")]
    [SerializeField]
    private float maxEffectRadius = 5.0f;
    [Tooltip("The max radius within which the cute and evil textures will be blended together once the building is conquered.")]
    [SerializeField]
    private float maxBlendedRadius = 35.0f;
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
    private void Reset()
    {
        alternateBuildingRenderer.transform.localScale = Vector3.zero;
        effectOnMapRadius = 0.0f;
        conquered = false;
    }

    private void Awake()
    {
        attachedBuilding = GetComponent<Building>();
        UnityEngine.Assertions.Assert.IsNotNull(attachedBuilding, "ERROR: A Building Component could not be found by BuildingEffects in GameObject " + gameObject.name);

        UnityEngine.Assertions.Assert.IsNotNull(buildingRenderer, "ERROR: Building Renderer not assigned for BuildingEffects in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateBuildingRenderer, "ERROR: Alternate Building Renderer not assigned for BuildingEffects script in GameObject " + gameObject.name);
        buildingRenderer.gameObject.SetActive(true);
        alternateBuildingRenderer.gameObject.SetActive(false);
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
                attachedBuilding.Conquer();
            }
        }
    }
    #endregion

    #region Public Methods
    public void StartConquerEffect()
    {
        if (!conquering)
        {
            conquering = true;
            conquerEffectElapsedTime = 0;
            alternateBuildingRenderer.transform.localScale = Vector3.zero;
            alternateBuildingRenderer.gameObject.SetActive(true);
        }
    }

    public void StartUnconquerEffect()
    {
        foreach (Convertible convertible in convertibles)
        {
            convertible.Unconvert();
        }

        alternateBuildingRenderer.transform.localScale = Vector3.zero;
        alternateBuildingRenderer.gameObject.SetActive(false);
        buildingRenderer.transform.localScale = Vector3.one;
        AdjustMaterials(0);

        Reset();
        conquered = false;
        attachedBuilding.Unconquer();
    }

    public float GetBlendRadius()
    {
        if (conquered)
            return maxEffectRadius / maxBlendedRadius;
        else if (conquering)
            return (conquerEffectElapsedTime / conquerEffectDuration) * (maxEffectRadius / maxBlendedRadius);
        else
            return 0.0f;
    }

    public void SetUnderAttack(bool underAttackState)
    {
        if (underAttackState)
            underAttackElapsedTime = 0;

        if (underAttack != underAttackState)
        {
            if (underAttackState)
            {
                buildingRenderer.material.SetFloat("_UnderAttack", 1);
            }
            else
            {
                buildingRenderer.material.SetFloat("_UnderAttack", 0);
            }
        }
        underAttack = underAttackState;
    }

    public void AdjustMaterials(float conquerFactor)
    {
        buildingRenderer.material.SetFloat("_ConquerFactor", conquerFactor);
        effectOnMapRadius = conquerFactor * maxEffectRadius;
    }
    #endregion

    #region Private Methods
    private void ConquerEffect()
    {
        float progress = conquerEffectElapsedTime / conquerEffectDuration;
        effectOnMapRadius = maxEffectRadius + progress * (maxBlendedRadius - maxEffectRadius);

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

        float limitRadius = maxEffectRadius * progress;
        foreach (Convertible convertible in convertibles)
        {
            if (!convertible.IsConverting() && Vector3.Distance(transform.position, convertible.transform.position) < limitRadius)
            {
                convertible.Convert();
            }
        }
    }

    #endregion
}
