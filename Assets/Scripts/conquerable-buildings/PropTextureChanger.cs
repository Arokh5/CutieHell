using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropTextureChanger : Convertible {

    private Renderer mRenderer;

    [Tooltip("The time (in seconds) it takes to blend from one texture to the other")]
    public float convertionDuration = 0.5f;
    private float convertionElapsedTime = 0;
    private bool isCute = false;
    private bool converting = false;
    private bool unconverting = false;
    

    private void Awake()
    {
        mRenderer = GetComponent<Renderer>();
        UnityEngine.Assertions.Assert.IsNotNull(mRenderer, "Renderer could not be found in PropTextureChanger in GameObject called " + gameObject.name);
    }

    private void Update()
    {
        if (converting || unconverting)
        {
            float blendFactor = convertionElapsedTime / convertionDuration;

            if (unconverting)
                blendFactor = 1 - blendFactor;

            mRenderer.material.SetFloat("_BlendFactor", blendFactor);
            convertionElapsedTime += Time.deltaTime;
            if (convertionElapsedTime >= convertionDuration)
            {
                convertionElapsedTime = 0;
                if (converting)
                {
                    converting = false;
                    isCute = true;
                }
                else if (unconverting)
                {
                    unconverting = false;
                    isCute = false;
                }
            }
        }
        
    }

    public override void Convert()
    {
        convertionElapsedTime = 0;
        converting = true;
    }

    public override void Unconvert()
    {
        convertionElapsedTime = 0;
        unconverting = true;
    }
}
