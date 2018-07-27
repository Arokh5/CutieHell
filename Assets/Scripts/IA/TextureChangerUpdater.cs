using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChangerUpdater : MonoBehaviour {

    #region Fields
    private TextureChangerSource textureChangerSource;
    private Renderer mRenderer;
    private Material[] materials;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        textureChangerSource = GetComponentInParent<TextureChangerSource>();
        UnityEngine.Assertions.Assert.IsNotNull(textureChangerSource, "ERROR: TextureChangerUpdater in gameObject '" + gameObject.name + "' doesn't have a TextureChangerSource assigned!");
        mRenderer = GetComponent<Renderer>();
        UnityEngine.Assertions.Assert.IsNotNull(mRenderer, "ERROR: TextureChangerUpdater in gameObject '" + gameObject.name + "' couldn't find a Renderer in its GameObject!");
        materials = mRenderer.materials;
    }

    private void Update()
    {
        foreach (Material material in materials)
            textureChangerSource.UpdateMaterial(material);
    }
    #endregion

#if UNITY_EDITOR
    private bool valid
    {
        get
        {
            return textureChangerSource != null && mRenderer != null && materials != null;
        }
    }

    public void EditorUpdate()
    {
        if (!Application.isPlaying)
        {
            if (!valid)
            {
                EditorAwake();
            }

            foreach (Material material in materials)
                textureChangerSource.UpdateMaterial(material);
        }
    }

    private void EditorAwake()
    {
        textureChangerSource = GetComponentInParent<TextureChangerSource>();
        UnityEngine.Assertions.Assert.IsNotNull(textureChangerSource, "ERROR: TextureChangerUpdater in gameObject '" + gameObject.name + "' doesn't have a TextureChangerSource assigned!");
        mRenderer = GetComponent<Renderer>();
        UnityEngine.Assertions.Assert.IsNotNull(mRenderer, "ERROR: TextureChangerUpdater in gameObject '" + gameObject.name + "' couldn't find a Renderer in its GameObject!");
        Material[] sharedMaterials = mRenderer.sharedMaterials;
        Material[] newMats = new Material[sharedMaterials.Length];
        for (int i = 0; i < sharedMaterials.Length; ++i)
        {
            newMats[i] = new Material(sharedMaterials[i]);
        }
        mRenderer.sharedMaterials = newMats;
        materials = newMats;
    }
    #endif
}
