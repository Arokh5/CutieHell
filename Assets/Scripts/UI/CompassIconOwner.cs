using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassIconOwner : MonoBehaviour {

    #region Fields
    [Tooltip("The sprite used for this GameObject in the compass")]
    public Sprite iconSprite;
    [Tooltip("The color that the alert light displays when this GameObject is in alert state but falls outside of the compass range")]
    public Color alertColor;
    [Tooltip("If more than one icon is in alert state and falls outside of the compass range (on the same side)," +
        "then the one with the highest importance takes preference and decides the light color")]
    public int iconImportance = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        UIManager.instance.compass.Register(this);
    }
    #endregion

    #region Public Methods
    public void Alert()
    {
        UIManager.instance.compass.SetAlertForIcon(this);
    }
    #endregion
}
