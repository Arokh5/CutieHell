using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTargets : MonoBehaviour {

    [SerializeField]
    private LayerMask enemiesLayer;
    public List<AIEnemy> currentMineTargets = new List<AIEnemy>();

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, enemiesLayer))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            currentMineTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Helpers.GameObjectInLayerMask(other.gameObject, enemiesLayer))
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            currentMineTargets.Remove(aIEnemy);
        }
    }
}
