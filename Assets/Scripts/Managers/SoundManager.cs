using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields
    public static SoundManager instance;

    [Header("Sources setup")]
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
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
	#endregion
	
	#region Public Methods

    public void PlayMusicClip(AudioClip musicClip, float pitch = 1f)
    {
        musicSource.clip = musicClip;
        musicSource.pitch = pitch;
        musicSource.Play();
    }

    public void PlaySfxClip(AudioClip sfxClip, float pitch = 1f)
    {
        if (CanPlay(sfxClip))
        {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(sfxClip);
        }
    }

    public void PlaySfxClip(AudioSource source, AudioClip clip = null, bool oneShot = false)
    {
        if (CanPlay(clip ? clip : source.clip))
        {
            if (oneShot)
            {
                if (clip)
                    source.PlayOneShot(clip);
                else
                    source.PlayOneShot(source.clip);
            }
            else
            {
                if (clip)
                    source.clip = clip;

                source.Play();
            }
        }
    }
    #endregion

    #region Private Methods
    private bool CanPlay(AudioClip clip)
    {
        return true;
    }
    #endregion
}
