using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingParticleController : MonoBehaviour {

    public Transform affector;

    private ParticleSystemRenderer psr;

	void Start () {
        psr = GetComponent<ParticleSystemRenderer>();
        affector = GameManager.instance.GetPlayer1().transform;
    }
	
	void Update () {
        psr.material.SetVector("_Affector", affector.position);
    }
}
