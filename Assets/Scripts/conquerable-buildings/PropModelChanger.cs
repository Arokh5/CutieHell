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
        UnityEngine.Assertions.Assert.IsNotNull(originalProp, "original Prop Mesh Renderer has NOT been assigned in GameObject called " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(alternateProp, "alternate Prop Mesh Renderer has NOT been assigned in GameObject called " + gameObject.name);
        originalProp.material.SetFloat("_Size", 1);
        alternateProp.material.SetFloat("_Size", 0);
    }

    private void Update()
    {
        if (converting)
        {
            if (convertionElapsedTime < propCollapseDuration)
            {
                float shrinkFactor = 1 - convertionElapsedTime / propCollapseDuration;
                originalProp.material.SetFloat("_Size", shrinkFactor);
            }
            else if (convertionElapsedTime < propCollapseDuration + alternatePropGrowDuration)
            {
                originalProp.material.SetFloat("_Size", 0);
                float growFactor = (convertionElapsedTime - propCollapseDuration) / alternatePropGrowDuration;
                alternateProp.material.SetFloat("_Size", growFactor);
            }
            else
            {
                // Convertion finished
                alternateProp.material.SetFloat("_Size", 1);
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
                alternateProp.material.SetFloat("_Size", shrinkFactor);
            }
            else if (convertionElapsedTime < alternatePropGrowDuration + propCollapseDuration)
            {
                originalProp.material.SetFloat("_Size", 0);
                float growFactor = (convertionElapsedTime - alternatePropGrowDuration) / propCollapseDuration;
                originalProp.material.SetFloat("_Size", growFactor);
            }
            else
            {
                // Unconvertion finished
                originalProp.material.SetFloat("_Size", 1);
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