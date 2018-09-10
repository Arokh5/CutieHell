using UnityEngine;

public class PropModelChanger : Convertible
{
    #region Fields
    [Header("Elements setup")]

    [SerializeField]
    private GameObject originalObject;
    [SerializeField]
    private GameObject alternateObject;

    [Header("Trigger mode")]
    [SerializeField]
    private bool triggerOnCuteGround = true;
    [SerializeField]
    private bool triggerOnFirstCuteContact = true;

    [Space]
    [Header("Change modes")]
    [SerializeField]
    private bool scaleChangeMode = false;

    [Header("Puff Change Mode")]
    [SerializeField]
    private ParticleSystem changeVFX;
    [SerializeField]
    private int numberOfParticles = 250;

    [Header("Scale Change Mode")]
    [SerializeField]
    [Tooltip("The time (in seconds) it takes to collapse the original prop model's vertices towards the pivot")]
    private float propCollapseDuration = 0.25f;
    [SerializeField]
    [Tooltip("The time (in seconds) it takes to move the alternate prop model's vertices from the pivot to their original position")]
    private float alternatePropGrowDuration = 0.75f;

    [Header("Scaling axes (Scale Change Mode)")]
    [SerializeField]
    private bool scaleX = true;
    [SerializeField]
    private bool scaleY = true;
    [SerializeField]
    private bool scaleZ = true;

    private float convertionElapsedTime;
    private TextureChangerSource textureChangerSource;
    #endregion

    #region MonoBehaviour Methods
    private void OnValidate()
    {
        if (!scaleX && !scaleY && !scaleZ)
        {
            scaleX = true;
            scaleY = true;
            scaleZ = true;
        }
    }

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(originalObject, "ERROR: original Prop Mesh Renderer has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateObject, "ERROR: alternate Prop Mesh Renderer has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);
        if (scaleChangeMode)
        {
            originalObject.transform.localScale = Vector3.one;
            originalObject.SetActive(true);
            alternateObject.transform.localScale = Vector3.zero;
            alternateObject.SetActive(true);
        }
        else
        {
            UnityEngine.Assertions.Assert.IsNotNull(changeVFX, "ERROR: Change VFX (ParticleSystem) has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);
            originalObject.transform.localScale = Vector3.one;
            alternateObject.transform.localScale = Vector3.one;
            originalObject.SetActive(true);
            alternateObject.SetActive(false);
        }
        textureChangerSource = GetComponentInParent<TextureChangerSource>();
        UnityEngine.Assertions.Assert.IsNotNull(textureChangerSource, "ERROR: Texture Changer Source (TextureChangerSource) could not be found in its parent hierarchy by GameObject '" + gameObject.name + "'!");
    }

    private void Update()
    {
        if (triggerOnCuteGround)
        {
            CheckCuteGround();
        }

        if (scaleChangeMode)
        {
            ScaleChangeMode();
        }
        else
        {
            PuffChangeMode();
        }
    }
    #endregion

    #region Public Methods
    public override void Convert()
    {
        if (triggerOnCuteGround)
        {
            Debug.LogWarning("WARNING: PropModelChanger::Convert called on GameObject '" + gameObject.name + "' which has triggerOnCuteGround set to true. The call to Convert will be ignored!");
        }
        else
        {
            if (!isConverted)
                converting = true;

        }
    }

    public override void Unconvert()
    {
        if (triggerOnCuteGround)
        {
            Debug.LogWarning("WARNING: PropModelChanger::Unconvert called on GameObject '" + gameObject.name + "' which has triggerOnCuteGround set to true. The call to Convert will be ignored!");
        }
        else
        {
            if (isConverted)
                unconverting = true;

        }
    }

    public void ForceEvil()
    {
        originalObject.transform.localScale = Vector3.one;
        alternateObject.transform.localScale = Vector3.one;
        originalObject.SetActive(true);
        alternateObject.SetActive(false);
    }

    public void ForceCute()
    {
        originalObject.transform.localScale = Vector3.one;
        alternateObject.transform.localScale = Vector3.one;
        originalObject.SetActive(false);
        alternateObject.SetActive(true);
    }
    #endregion

    #region Private Methods
    private Vector3 GetScale(float factor)
    {
        return new Vector3(scaleX ? factor : 1, scaleY ? factor : 1, scaleZ ? factor : 1);
    }

    private void ScaleChangeMode()
    {
        if (converting)
        {
            if (convertionElapsedTime < propCollapseDuration)
            {
                float shrinkFactor = 1 - convertionElapsedTime / propCollapseDuration;
                originalObject.transform.localScale = GetScale(shrinkFactor);
            }
            else if (convertionElapsedTime < propCollapseDuration + alternatePropGrowDuration)
            {
                originalObject.transform.localScale = Vector3.zero;
                float growFactor = (convertionElapsedTime - propCollapseDuration) / alternatePropGrowDuration;
                alternateObject.transform.localScale = GetScale(growFactor);
            }
            else
            {
                // Convertion finished
                alternateObject.transform.localScale = Vector3.one;
                convertionElapsedTime = 0;
                converting = false;
                isConverted = true;
            }
        }
        else if (unconverting)
        {
            if (convertionElapsedTime < alternatePropGrowDuration)
            {
                float shrinkFactor = 1 - convertionElapsedTime / alternatePropGrowDuration;
                alternateObject.transform.localScale = GetScale(shrinkFactor);
            }
            else if (convertionElapsedTime < alternatePropGrowDuration + propCollapseDuration)
            {
                alternateObject.transform.localScale = Vector3.zero;
                float growFactor = (convertionElapsedTime - alternatePropGrowDuration) / propCollapseDuration;
                originalObject.transform.localScale = GetScale(growFactor);
            }
            else
            {
                // Unconvertion finished
                originalObject.transform.localScale = Vector3.one;
                convertionElapsedTime = 0;
                unconverting = false;
                isConverted = false;
            }
        }

        if (converting || unconverting)
        {
            convertionElapsedTime += Time.deltaTime;
        }
    }

    private void PuffChangeMode()
    {
        if (converting && originalObject.activeSelf)
        {
            originalObject.SetActive(false);
            alternateObject.SetActive(true);
            foreach (MeshFilter meshFilter in alternateObject.GetComponentsInChildren<MeshFilter>())
            {
                ParticleSystem ps = ParticlesManager.instance.LaunchParticleSystem(changeVFX, alternateObject.transform.position, alternateObject.transform.rotation);
                ParticleSystem.Burst burst = new ParticleSystem.Burst(0.0f, numberOfParticles);
                ps.emission.SetBurst(0, burst);
                ParticleSystem.ShapeModule shape = ps.shape;
                shape.mesh = meshFilter.mesh;
            }
            converting = false;
            isConverted = true;
        }
    }

    private void CheckCuteGround()
    {
        if (converting || isConverted)
            return;

        bool inCuteArea = true;
        foreach (ITextureChanger textureChanger in textureChangerSource.textureChangers)
        {
            Vector3 propToChanger = textureChanger.transform.position - transform.position;
            float safeRadius = textureChanger.GetEffectMaxRadius();
            if (triggerOnFirstCuteContact)
            {
                safeRadius *= textureChanger.GetNormalizedBlendStartRadius();
            }
            if (propToChanger.sqrMagnitude < safeRadius * safeRadius)
            {
                inCuteArea = false;
                break;
            }
        }
        if (inCuteArea)
        {
            if (!isConverted)
                converting = true;
        }
    }
    #endregion
}