using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class LiveParticles : MonoBehaviour {

    public Transform target;
    public float changeColorDistance = 5;
    public float changeColorLength = 2;

    new ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;

    ParticleSystem.MainModule particleSystemMainModule;

    void Start () {
        particleSystem = this.GetComponent<ParticleSystem>();
        particleSystemMainModule = particleSystem.main;
    }
	
	void LateUpdate () {
        int maxParticles = particleSystemMainModule.maxParticles;

        if (particles == null || particles.Length < maxParticles) {
            particles = new ParticleSystem.Particle[maxParticles];
        }

        particleSystem.GetParticles(particles);
        int particleCount = particleSystem.particleCount;
        float sqrDistance = changeColorDistance * changeColorDistance;

        for(int i = 0; i < particleCount; i++) {
            if(sqrDistance > Vector3.SqrMagnitude(target.position - particles[i].position)) {
                //particles[i].position += Vector3.up * Time.deltaTime;
                float distance = Vector3.Distance(target.position, particles[i].position);
                float firstColorAmmount = distance - (changeColorDistance - changeColorLength);
                firstColorAmmount /= changeColorLength;
                particles[i].startColor = MixColors(firstColorAmmount, Color.red, Color.blue);
            }
        }
        particleSystem.SetParticles(particles, particles.Length);
    }

    Color MixColors(float color1_ammount, Color color_1, Color color_2) {
        Color result;
        result = color_1 * color1_ammount + color_2 * (1 - color1_ammount);
        return result;
    }
}
