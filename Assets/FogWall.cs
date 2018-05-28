using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWall : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            other.GetComponent<AIEnemy>().TakeDamage(200, AttackType.NONE);
        }
    }
}
