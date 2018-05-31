using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlexus : MonoBehaviour {

    public float maxDistance = 1.0f;

    public LineRenderer lineRendererTemplate;
    List<LineRenderer> lineRenderers = new List<LineRenderer>();

    new ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;

    ParticleSystem.MainModule particleSystemMainModule;

    Transform _transform;

	void Start () {
        particleSystem = this.GetComponent<ParticleSystem>();
        particleSystemMainModule = particleSystem.main;
        _transform = this.transform;
	}
	
	void LateUpdate () {
        int maxParticles = particleSystemMainModule.maxParticles;

        if(particles == null || particles.Length < maxParticles) {
            particles = new ParticleSystem.Particle[maxParticles];
        }

        particleSystem.GetParticles(particles);
        int particleCount = particleSystem.particleCount;

        float maxDistanceSqr = maxDistance * maxDistance;

        int lrIndex = 0;
        int lineRemdererCount = lineRenderers.Count;
        
        for(int i = 0; i < particleCount; i++) {

            Vector3 p1_position = particles[i].position;
            for (int j = i + 1; j < particleCount; j++) {
                Vector3 p2_position = particles[j].position;
                float distanceSqr = Vector3.SqrMagnitude(p1_position - p2_position);
                if(distanceSqr <= maxDistanceSqr) {
                    if(lrIndex == lineRemdererCount) {
                        LineRenderer lr = Instantiate(lineRendererTemplate, _transform, false);
                        lineRenderers.Add(lr);
                        lineRemdererCount++;
                    }

                    lineRenderers[lrIndex].enabled = true;

                    lineRenderers[lrIndex].SetPosition(0, p1_position);
                    lineRenderers[lrIndex].SetPosition(1, p2_position);
                    lrIndex++;
                }
            }
        }

        for(int i = lrIndex; i < lineRemdererCount; i++) {
            lineRenderers[i].enabled = false;
        }

    }
}
