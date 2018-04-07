using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChangerUpdater : MonoBehaviour {

    #region Fields
    public AIZoneController zoneController;
    private Renderer mRenderer;
    private Material material;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(zoneController, "ERROR: TextureChangerUpdater in gameObject '" + gameObject.name + "' doesn't have an AIZoneController assigned!");
        mRenderer = GetComponent<Renderer>();
        UnityEngine.Assertions.Assert.IsNotNull(mRenderer, "ERROR: TextureChangerUpdater in gameObject '" + gameObject.name + "' couldn't find a Renderer in its GameObject!");
        material = mRenderer.material;
    }

    private void Update()
    {
        zoneController.UpdateMaterialWithEnemyPositions(material);
    }
    #endregion
}
