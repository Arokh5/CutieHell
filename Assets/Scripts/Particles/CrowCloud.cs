using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowCloud : MonoBehaviour {

    [SerializeField]
    private Transform gearOne;
    [SerializeField]
    private Transform gearTwo;
    [SerializeField]
    private Transform gearThree;
    [SerializeField]
    private Transform gearFour;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gearOne.Rotate(Vector3.up * 52.0f * Time.deltaTime * 3);
        gearTwo.Rotate(Vector3.up * 64.0f * Time.deltaTime * 3);
        gearThree.Rotate(Vector3.up * 75.0f * Time.deltaTime * 3);
        gearFour.Rotate(Vector3.up * 90.0f * Time.deltaTime * 3);
    }
}
