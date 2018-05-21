using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChangerUpdater : MonoBehaviour {

    #region Fields
    public TextureChangerSource textureChangerSource;
    private Renderer mRenderer;
    private Material material;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (textureChangerSource == null)
            textureChangerSource = GetComponentInParent<TextureChangerSource>();
        UnityEngine.Assertions.Assert.IsNotNull(textureChangerSource, "ERROR: TextureChangerUpdater in gameObject '" + gameObject.name + "' doesn't have a TextureChangerSource assigned!");
        mRenderer = GetComponent<Renderer>();
        UnityEngine.Assertions.Assert.IsNotNull(mRenderer, "ERROR: TextureChangerUpdater in gameObject '" + gameObject.name + "' couldn't find a Renderer in its GameObject!");
        material = mRenderer.material;
    }

    private void Update()
    {
        textureChangerSource.UpdateMaterial(material);
    }
    #endregion
}
