using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageCounter : MonoBehaviour {

    [SerializeField]
    private Text textPercentage;

    public void UpdatePercentage(float newPercentage)
    {

        textPercentage.text = newPercentage.ToString("0") + "%";
    }
	
}
