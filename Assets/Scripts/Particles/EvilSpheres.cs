using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSpheres : PooledParticleSystem
{
    #region Fields
    public int particlesToSpawn;
    public int evilReward;
    public Player player;

    private ParticleSystem ps;
    private float timeToFollow;
    private float deathDelay;
    private float timer;
    private int evilDelivered;
    #endregion

    #region MonoBehaviour Methods
    private void Awake ()
    {
        ps = this.GetComponent<ParticleSystem>();
        timeToFollow = 1.2f;
        deathDelay = 0.5f;
	}

    public void Emit()
    {
        var em = ps.emission;
        em.burstCount = 1;
        em.SetBurst(0, new ParticleSystem.Burst(0.1f, particlesToSpawn));
    }

    private void Update ()
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
                    Debug.Log("Enemies Death is no longer giving evil to the player");
                    //SetEvil(particles.Length);
                    particles[i].remainingLifetime = 0;
                }
                else
                {
                    particles[i].velocity = Vector3.Lerp(particles[i].velocity, (player.transform.position + Vector3.up - particles[i].position).normalized * 20, 0.175f);
                }
            }

            ps.SetParticles(particles, particles.Length);

            if (particles.Length == 0)
            {
                timer += Time.deltaTime;
                if (timer > timeToFollow + deathDelay)
                    ReturnToPool();
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
	}
    #endregion

    #region Public Methods
    public override void Restart()
    {
        timer = 0;
        evilDelivered = 0;
    }
    #endregion

    #region Private Methods
    private void SetEvil(int particlesLeft)
    {
        if (particlesLeft > 1)
        {
            int evilToDeliver = evilReward / particlesToSpawn;
            player.AddEvilPoints(evilToDeliver);
            evilDelivered += evilToDeliver;
        }
        else
        {
            player.AddEvilPoints(evilReward - evilDelivered);
        }
    }
    #endregion
}
