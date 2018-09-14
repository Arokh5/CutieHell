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

    [Header("Puff setup")]
    [SerializeField]
    private ParticleSystem changeVFX;
    [SerializeField]
    private int numberOfParticles = 250;

    private TextureChangerSource textureChangerSource;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(originalObject, "ERROR: original Prop Mesh Renderer has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateObject, "ERROR: alternate Prop Mesh Renderer has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);

        UnityEngine.Assertions.Assert.IsNotNull(changeVFX, "ERROR: Change VFX (ParticleSystem) has NOT been assigned in PropModelChanger in GameObject called " + gameObject.name);
        originalObject.SetActive(true);
        alternateObject.SetActive(false);

        textureChangerSource = GetComponentInParent<TextureChangerSource>();
        UnityEngine.Assertions.Assert.IsNotNull(textureChangerSource, "ERROR: Texture Changer Source (TextureChangerSource) could not be found in its parent hierarchy by GameObject '" + gameObject.name + "'!");
    }

    private void Update()
    {
        if (triggerOnCuteGround)
        {
            CheckCuteGround();
        }

        PuffChangeMode();
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
        originalObject.SetActive(true);
        alternateObject.SetActive(false);
    }

    public void ForceCute()
    {
        originalObject.SetActive(false);
        alternateObject.SetActive(true);
    }
    #endregion

    #region Private Methods
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
                float unitScale = alternateObject.transform.localScale.x * alternateObject.transform.parent.transform.localScale.x;
                shape.scale = new Vector3(unitScale, unitScale, unitScale);
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