using UnityEngine;

public class FogWall : MonoBehaviour {

    [SerializeField]
    private LayerMask enemiesLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, enemiesLayer))
        {
            other.GetComponent<AIEnemy>().TakeDamage(200, AttackType.NONE);
        }
    }
}
