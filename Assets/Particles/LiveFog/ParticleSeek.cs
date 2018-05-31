using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSeek : MonoBehaviour {

    public Transform target;
    public float force = 10.0f;

    private ParticleSystem ps;
	
	void Start () {

        ps = this.GetComponent<ParticleSystem>();
	}
	
	void LateUpdate () {

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(particles);

        for(int i = 0; i < particles.Length; i++) {

            Vector3 direction = (target.position - particles[i].position).normalized;
            Vector3 seekForce = direction * force * Time.deltaTime / (Mathf.Clamp(particles[i].remainingLifetime,0.1f,1.0f) / 2);

            particles[i].velocity += seekForce;

        }
        ps.SetParticles(particles, ps.particleCount);

	}
}
