using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoritesStorm : MonoBehaviour {

    public float timeDelay, attackDuration, delayOnMeteoritesLaunch;
    public ParticleSystem meteoriteVFX, spawnVFX;
    private float timeSinceLastMeteoriteLaunch;
    private Player player;
 
    void OnEnable ()
    {
        player = GameManager.instance.GetPlayer1();
        timeDelay = 2.0f;
        attackDuration = 6.5f;
        timeSinceLastMeteoriteLaunch = 0.0f;
        player.SetIsMeteoritesOn(true);
	}
	
	void Update ()
    {
		if(timeDelay >= 0.0f)
        {
            timeDelay -= Time.deltaTime;
        }
        else
        {
            if(attackDuration >= 0.0f)
            {
                attackDuration -= Time.deltaTime;
                timeSinceLastMeteoriteLaunch += Time.deltaTime;

                if(timeSinceLastMeteoriteLaunch >= delayOnMeteoritesLaunch)
                {
                    timeSinceLastMeteoriteLaunch = 0.0f;
                    float angle = Random.Range(0, Mathf.PI * 2);
                    float x = Mathf.Sin(angle) * Random.Range(0.0f, 9.0f);
                    float z = Mathf.Cos(angle) * Random.Range(0.0f, 9.0f);
                    x += this.transform.position.x;
                    z += this.transform.position.z;
                    ParticleSystem ps = ParticlesManager.instance.LaunchParticleSystem(meteoriteVFX, new Vector3(x , this.transform.position.y + 5.0f ,z), Quaternion.LookRotation(Vector3.down));
                    ParticleSystem spawn = ParticlesManager.instance.LaunchParticleSystem(spawnVFX, new Vector3(x, this.transform.position.y, z), spawnVFX.transform.rotation);
                }
            }
            else
            {
                player.SetIsMeteoritesOn(false);
            }
        }
	}
}
