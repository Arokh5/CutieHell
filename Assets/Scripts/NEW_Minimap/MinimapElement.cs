using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapElement : MonoBehaviour
{
    public Sprite sprite;
    public Color color = Color.white;

    #region MonoBehaviour Methods
    private void Start()
    {
        MinimapController.instance.AddMinimapElement(this);
    }

    private void OnDestroy()
    {
        MinimapController.instance.AddMinimapElement(this);
    }
    #endregion
}
