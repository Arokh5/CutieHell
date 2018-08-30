using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSoundOnTime : MonoBehaviour {

    public float timeToSpawn;
    public AudioSource audioSource;
    public AudioClip audioClip;
    private float timer = 0.0f;
    private bool flag = false;

	// Use this for initialization
	void OnEnable () {
        timer = 0.0f;
        flag = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!flag)
        {
            timer += Time.deltaTime;
            if (timer >= timeToSpawn)
            {
                SoundManager.instance.PlaySfxClip(audioSource, audioClip, true);
                flag = true;
            }
        }
	}
}
