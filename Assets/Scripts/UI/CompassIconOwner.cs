using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassIconOwner : MonoBehaviour {

    #region Fields
    public Sprite iconSprite;
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
