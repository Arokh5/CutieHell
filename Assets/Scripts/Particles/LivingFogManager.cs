using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingFogManager : MonoBehaviour {

    [SerializeField]
    private FogWall[] fogWalls;
    [SerializeField]
    private GameObject[] deactivatedVFX;

    void Start () {
		
	}

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < fogWalls.Length; i++)
            {
                DeactivateFogWall(i);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < fogWalls.Length; i++)
            {
                ActivateFogWall(i);
            }
        }
	}

    public void DeactivateFogWall(int index)
    {
        fogWalls[index].gameObject.SetActive(false);
        deactivatedVFX[index].SetActive(true);
    }
    public void ActivateFogWall(int index)
    {
        fogWalls[index].gameObject.SetActive(true);
        deactivatedVFX[index].SetActive(false);
    }
}
