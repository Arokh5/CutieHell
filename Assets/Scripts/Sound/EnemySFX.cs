using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private AudioSource walkSource;
    [SerializeField]
    private AudioClip walkClip;

    #endregion

    #region Properties

    public AudioSource GetWalkSource()
    {
        return walkSource;
    }

    #endregion

    #region MonoBehaviour Methods
    
    private void Awake()
    {
        walkSource.clip = walkClip;
    }

    #endregion
}