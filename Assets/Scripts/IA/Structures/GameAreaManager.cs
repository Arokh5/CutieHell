using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameAreaManager : MonoBehaviour {

    [Header("Area Battlefield elements")]
    [SerializeField]
    public ConquerableElement defensePoint;
    

    private List<ConquerableElement> traps;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
