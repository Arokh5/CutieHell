using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropModelChanger : Convertible
{
    [Header("Elements setup")]

    [SerializeField]
    private MeshRenderer originalProp;
    [SerializeField]
    private MeshRenderer alternateProp;

    [Tooltip("The time (in seconds) it takes to collapse the original prop model's vertices towards the pivot")]
    public float propCollapseDuration = 0.25f;
    [Tooltip("The time (in seconds) it takes to move the alternate prop model's vertices from the pivot to their original position")]
    public float alternatePropGrowDuration = 0.75f;

    private float convertionElapsedTime;

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
                originalProp.transform.localScale = shrinkFactor * Vector3.one;
            }
            else if (convertionElapsedTime < propCollapseDuration + alternatePropGrowDuration)
            {
                originalProp.transform.localScale = Vector3.zero;
                float growFactor = (convertionElapsedTime - propCollapseDuration) / alternatePropGrowDuration;
                alternateProp.transform.localScale = growFactor * Vector3.one;
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
                alternateProp.transform.localScale = shrinkFactor * Vector3.one;
            }
            else if (convertionElapsedTime < alternatePropGrowDuration + propCollapseDuration)
            {
                alternateProp.transform.localScale = Vector3.zero;
                float growFactor = (convertionElapsedTime - alternatePropGrowDuration) / propCollapseDuration;
                originalProp.transform.localScale = growFactor * Vector3.one;
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

    public override void Convert()
    {
        converting = true;
    }

    public override void Unconvert()
    {
        unconverting = true;
    }

}