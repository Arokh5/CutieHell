using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields

    public static Player instance;

    private const int maxEvilLevel = 20;
    private int evilLevel = maxEvilLevel;

	#endregion
	
	#region Properties

    public int GetMaxEvilLevel()
    {
        return maxEvilLevel;
    }

    public int GetEvilLevel()
    {
        return evilLevel;
    }

    public void SetEvilLevel(int value)
    {
        evilLevel += value;

        if (evilLevel < 0)
        {
            evilLevel = 0;
        }
        else if (evilLevel > maxEvilLevel)
        {
            evilLevel = maxEvilLevel;
        }

        UIManager.instance.SetEvilBarValue(evilLevel);
    }

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion
}