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
    private AudioSource ambienceSource;
    [SerializeField]
    private AudioSource sfxSource;

	#endregion
	
	#region MonoBehaviour Methods
	
    private void Awake()
    {
        if (instance == null) instance = this;
    }

	#endregion
	
	#region Public Methods

    public void PlayMusicClip(AudioClip musicClip, float pitch = 1f)
    {
        musicSource.clip = musicClip;
        musicSource.pitch = pitch;
        musicSource.Play();
    }

    public void PlaySfxClip(AudioClip efxClip, float pitch = 1f)
    {
        sfxSource.clip = efxClip;
        sfxSource.pitch = pitch;
        sfxSource.Play();
    }

    #endregion
}