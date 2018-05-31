using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingParticleController : MonoBehaviour {

    public Transform affector;

    private ParticleSystemRenderer psr;

	void Start () {
        psr = GetComponent<ParticleSystemRenderer>();
        affector = GameObject.Find("Player").transform;
	}
	
	void Update () {
        psr.material.SetVector("_Affector", affector.position);
    }
}
