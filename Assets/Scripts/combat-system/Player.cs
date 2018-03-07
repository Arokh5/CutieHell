using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields

    public static Player instance;

    private const int maxEvilLevel = 20;
    private int evilnessLevel = maxEvilLevel;

	#endregion
	
	#region Properties

    public int GetMaxEvilLevel()
    {
        return maxEvilLevel;
    }
	
    public void SetEvilnessLevel(int evilness)
    {
        evilnessLevel += evilness;
        UIManager.instance.SetEvilBarValue(evilness);
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