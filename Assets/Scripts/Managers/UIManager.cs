using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields

    public static UIManager instance;

    [SerializeField]
    private GameObject evilnessBar;

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

    #endregion

    #region Private Methods

    // Called by Player when using or earning Evil Points
    public void SetEvilBarValue(int value)
    {
        Debug.Log("Evil: " + ((float)value / Player.instance.GetMaxEvilLevel()));
        evilnessBar.GetComponent<Image>().fillAmount = ((float)value / Player.instance.GetMaxEvilLevel());
    }

    #endregion
}