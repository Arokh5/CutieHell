using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallMotion : MonoBehaviour
{
    
    [HideInInspector]
    public float canonBallElapsedTime = 0;
    [HideInInspector]
    public float canonBallFiringTime = 0;
    [HideInInspector]
    public float canonBallShootingDuration;
    [HideInInspector]
    public Vector3 canonBallShotingDistance;
    [HideInInspector]
    public float canonBallVisibleFromProgression;

    public Renderer canonBallRenderer;

    private bool alreadyFired = false;
    private bool hasToExplode = false;

    #region MonoBehaviour methods
    private void Start()
    {
        canonBallRenderer = gameObject.GetComponent<Renderer>();
    }
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

    public bool GetAlreadyFired()
    {
        return alreadyFired;
    }

    public void SetAlreadyFired(bool alreadyFiredValue)
    {
        alreadyFired = alreadyFiredValue;
    }
    #endregion

}