using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields

    public static SoundManager instance;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource efxSource;

	#endregion
	
	#region MonoBehaviour Methods
	
    private void Awake()
    {
        if (instance == null) instance = this;
    }

	#endregion
	
	#region Public Methods
	
    public void PlayMusicClip(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlayEfxClip(AudioClip efxClip)
    {
        efxSource.clip = efxClip;
        efxSource.Play();
    }

	#endregion
}