using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingFogManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] fogWalls;
    [SerializeField]
    private GameObject[] deactivatedVFX;

    public void DeactivateFogWall(int index)
    {
        fogWalls[index].SetActive(false);
        deactivatedVFX[index].SetActive(true);
    }
    public void ActivateFogWall(int index)
    {
        fogWalls[index].SetActive(true);
        deactivatedVFX[index].SetActive(false);
    }
}
