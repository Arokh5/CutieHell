using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTargets : MonoBehaviour {

    public List<AIEnemy> currentMineTargets = new List<AIEnemy>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            currentMineTargets.Add(aIEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            AIEnemy aIEnemy = other.GetComponent<AIEnemy>();
            currentMineTargets.Remove(aIEnemy);
        }
    }
}
