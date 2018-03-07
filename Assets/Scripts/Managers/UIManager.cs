using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    #region Public Data

    public static UIManager instance;

    #endregion

    #region Private Serialized Fields

    #endregion

    #region Private Non-Serialized Fields

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Public Methods

    public void SetEvilBarValue(int value)
    {
        
    }

    #endregion

    #region Private Methods

    #endregion
}