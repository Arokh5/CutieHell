using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneConnection : MonoBehaviour {

    #region Fields
    private List<AIEnemy> aiEnemiesInConnection = new List<AIEnemy>();
    #endregion

    #region MonoBehavior Methods
    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        AIEnemy aiEnemy = other.GetComponent<AIEnemy>();
        if (aiEnemy)
        {
            aiEnemiesInConnection.Add(aiEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AIEnemy aiEnemy = other.GetComponent<AIEnemy>();
        if (aiEnemy)
        {
            aiEnemiesInConnection.Remove(aiEnemy);
        }
    }
    #endregion

    #region Public Methods
    // Called by one of two Zonecontroller when its Monument gets conquered
    public void Open()
    {
        Debug.LogError("NOT IMPLEMENTED: ZoneConnection::Open");
    }

    // Called by one of two Zonecontroller when its Monument gets repaired
    public void Close()
    {
        Debug.LogError("NOT IMPLEMENTED: ZoneConnection::Close");
    }

    public bool ContainsEnemy(AIEnemy enemy)
    {
        return aiEnemiesInConnection.Contains(enemy);
    }
    #endregion
}
