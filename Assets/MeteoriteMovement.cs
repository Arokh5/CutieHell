using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteMovement : MonoBehaviour {

    public float speed = 8.0f;
    public float damage = 4.0f;
    [SerializeField]
    private ParticleSystem damageDealer;
    [SerializeField]
    private TimedPooledParticleSystem pool;

    void Update () {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 17)
        {
            MeteoriteDamageTrigger o = ParticlesManager.instance.LaunchParticleSystem(damageDealer, this.transform.position, damageDealer.transform.rotation).GetComponent<MeteoriteDamageTrigger>();
            o.damage = 3.0f;
            pool.ReturnToPool();
        }

    }
}
