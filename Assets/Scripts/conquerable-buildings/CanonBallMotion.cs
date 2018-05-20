using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallMotion : MonoBehaviour
{

    public GameObject canonBall;
    public float canonBallElapsedTime;
    public float canonBallShootingDuration;
    public Vector3 canonBallShotingDistance;
    public float canonBallVisibleFromProgression;

    private bool hasToExplode = false;

    #region MonoBehaviour methods
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("Enemy"))
        {
            hasToExplode = true;
        }
    }
    #endregion

    #region Public methods
    public bool GetHasToExplode()
    {
        return hasToExplode;
    }
    #endregion

}