using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteMovement : MonoBehaviour {

    [SerializeField]
    private LayerMask explodeLayer;
    public float speed = 8.0f;
    public float damage = 4.0f;
    [SerializeField]
    private ParticleSystem damageDealer;
    [SerializeField]
    private TimedPooledParticleSystem pool;

    private Player player;

    private void Start()
    {
        player = GameManager.instance.GetPlayer1();
    }

    void Update () {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, explodeLayer))
        {
            MeteoriteDamageTrigger o = ParticlesManager.instance.LaunchParticleSystem(damageDealer, this.transform.position, damageDealer.transform.rotation).GetComponent<MeteoriteDamageTrigger>();
            o.damage = 3.0f;
            pool.ReturnToPool();
        }
    }
}
