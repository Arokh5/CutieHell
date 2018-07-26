using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChangerSource : MonoBehaviour
{
    #region Fields
    [HideInInspector]
    public List<ITextureChanger> textureChangers;
    private int maxElements = 128;  // IMPORTANT: This number must be reflected in the TextureChanger.shader file
    private Vector4[] elements;
    private float[] normalizedBlendStartRadii;
    private int activeElements = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        textureChangers = new List<ITextureChanger>(128);
        elements = new Vector4[maxElements];
        normalizedBlendStartRadii = new float[maxElements];
        activeElements = 0;
    }

    private void Update()
    {
        /* Shader data update */
        activeElements = textureChangers.Count;
        for (int i = 0; i < textureChangers.Count && i < maxElements; ++i)
        {
            elements[i].x = textureChangers[i].transform.position.x;
            elements[i].y = textureChangers[i].transform.position.y;
            elements[i].z = textureChangers[i].transform.position.z;
            elements[i].w = textureChangers[i].GetEffectMaxRadius();
            normalizedBlendStartRadii[i] = textureChangers[i].GetNormalizedBlendStartRadius();
        }
    }
    #endregion

    #region Public Methods
    public void UpdateMaterial(Material material)
    {
        material.SetInt("_ActiveElements", activeElements);
        material.SetVectorArray("_Elements", elements);
        material.SetFloatArray("_NormalizedBlendStartRadii", normalizedBlendStartRadii);
    }

    public void AddTextureChanger(ITextureChanger textureChanger)
    {
        if (!textureChangers.Contains(textureChanger))
        {
            textureChangers.Add(textureChanger);
            UnityEngine.Assertions.Assert.IsTrue(textureChangers.Count < maxElements, "ERROR: The amount of TextureChangers exceeds the maximum amount of elements accepted by the TextureChanger shader (" + maxElements + "). The elements exceeding the maximum will be ignored!");
        }
    }

    public bool RemoveTextureChanger(ITextureChanger textureChanger)
    {
        return textureChangers.Remove(textureChanger);
    }
    #endregion
}
