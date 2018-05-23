using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSpheres : MonoBehaviour {

    //public Transform playerPos;
    public int particlesToSpawn;
    public int evilReward;
    public Player player;

    private ParticleSystem ps;
    private float timeToFollow;
    private float timer;
    private float currentSpeed;

	void Start ()
    {
        ps = this.GetComponent<ParticleSystem>();

        var em = ps.emission;
        em.burstCount = 1;
        em.SetBurst(0,new ParticleSystem.Burst(0.1f, particlesToSpawn));

        timeToFollow = 1.2f;
        timer = 0;
	}
	
	void Update ()
    {
        if (timer >= timeToFollow)
        {
            ParticleSystem.Particle[] particles =
                new ParticleSystem.Particle[ps.particleCount];

            ps.GetParticles(particles);

            for (int i = 0; i < particles.Length; i++)
            {
                if (Vector3.SqrMagnitude(player.transform.position + Vector3.up - particles[i].position) < 1.0f)
                {
                    SetEvil();
                    particles[i].remainingLifetime = 0;
                }
                else
                {
                    particles[i].velocity = Vector3.Lerp(particles[i].velocity, (player.transform.position + Vector3.up - particles[i].position).normalized * 20, 0.175f);
                }
            }

            ps.SetParticles(particles, particles.Length);
        }
        else
        {
            timer += Time.deltaTime;
        }
	}

    private void SetEvil()
    {
        player.AddEvilPoints(evilReward / particlesToSpawn);
    }
}
