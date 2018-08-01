using UnityEngine;

public class PropModelChanger : Convertible
{
    #region Fields
    [Header("Elements setup")]

    [SerializeField]
    private MeshRenderer originalProp;
    [SerializeField]
    private MeshRenderer alternateProp;
    
    [Header("Timing")]
    [Tooltip("The time (in seconds) it takes to collapse the original prop model's vertices towards the pivot")]
    public float propCollapseDuration = 0.25f;
    [Tooltip("The time (in seconds) it takes to move the alternate prop model's vertices from the pivot to their original position")]
    public float alternatePropGrowDuration = 0.75f;

    [Header("Scaling axes")]
    public bool scaleX = true;
    public bool scaleY = true;
    public bool scaleZ = true;

    private float convertionElapsedTime;
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
        originalProp.transform.localScale = Vector3.one;
        originalProp.gameObject.SetActive(true);
        alternateProp.transform.localScale = Vector3.zero;
        alternateProp.gameObject.SetActive(true);
    }

    private void Update()
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
    #endregion

    #region Public Methods
    public override void Convert()
    {
        if (!isConverted)
            converting = true;
    }

    public override void Unconvert()
    {
        if (isConverted)
            unconverting = true;
    }
    #endregion

    #region Private Methods
    private Vector3 GetScale(float factor)
    {
        return new Vector3(scaleX ? factor : 1, scaleY ? factor : 1, scaleZ ? factor : 1);
    }
    #endregion
}