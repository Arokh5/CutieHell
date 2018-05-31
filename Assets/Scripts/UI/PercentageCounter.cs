using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageCounter : MonoBehaviour {
    
    #region Fields
    [SerializeField]
    private Text textPercentage;
    #endregion

    #region PublicMethods
    public void UpdatePercentage(float newPercentage)
    {
        textPercentage.text = newPercentage.ToString("0") + "%";
    }
    #endregion

}
