using UnityEngine;

public class PropModelChanger : Convertible
{
    #region Fields
    [Header("Elements setup")]

    [SerializeField]
    private MeshRenderer originalProp;
    [SerializeField]
    private MeshRenderer alternateProp;

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
        UnityEngine.Assertions.Assert.IsNotNull(originalProp, "ERROR: original Prop Mesh Renderer has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateProp, "ERROR: alternate Prop Mesh Renderer has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);
        if (scaleChangeMode)
        {
            originalProp.transform.localScale = Vector3.one;
            originalProp.gameObject.SetActive(true);
            alternateProp.transform.localScale = Vector3.zero;
            alternateProp.gameObject.SetActive(true);
        }
        else
        {
            UnityEngine.Assertions.Assert.IsNotNull(changeVFX, "ERROR: Change VFX (ParticleSystem) has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);
            originalProp.gameObject.SetActive(true);
            alternateProp.gameObject.SetActive(false);
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
                originalProp.transform.localScale = GetScale(shrinkFactor);
            }
            else if (convertionElapsedTime < propCollapseDuration + alternatePropGrowDuration)
            {
                originalProp.transform.localScale = Vector3.zero;
                float growFactor = (convertionElapsedTime - propCollapseDuration) / alternatePropGrowDuration;
                alternateProp.transform.localScale = GetScale(growFactor);
            }
            else
            {
                // Convertion finished
                alternateProp.transform.localScale = Vector3.one;
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
                alternateProp.transform.localScale = GetScale(shrinkFactor);
            }
            else if (convertionElapsedTime < alternatePropGrowDuration + propCollapseDuration)
            {
                alternateProp.transform.localScale = Vector3.zero;
                float growFactor = (convertionElapsedTime - alternatePropGrowDuration) / propCollapseDuration;
                originalProp.transform.localScale = GetScale(growFactor);
            }
            else
            {
                // Unconvertion finished
                originalProp.transform.localScale = Vector3.one;
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
        if(converting && originalProp.gameObject.activeSelf)
        {
            originalProp.gameObject.SetActive(false);
            alternateProp.gameObject.SetActive(true);
            ParticleSystem ps = ParticlesManager.instance.LaunchParticleSystem(changeVFX, alternateProp.transform.position, alternateProp.transform.rotation);
            ParticleSystem.Burst burst = new ParticleSystem.Burst(0.0f, numberOfParticles);
            ps.emission.SetBurst(0, burst);
            ParticleSystem.ShapeModule shape = ps.shape;
            shape.mesh = alternateProp.GetComponent<MeshFilter>().mesh;
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