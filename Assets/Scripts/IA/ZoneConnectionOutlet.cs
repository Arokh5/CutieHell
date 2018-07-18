using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneConnectionOutlet : MonoBehaviour
{
    #region Fields
    private ZoneConnection zoneConnection;
    public AIZoneController outletZoneController;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        zoneConnection = GetComponentInParent<ZoneConnection>();
        UnityEngine.Assertions.Assert.IsNotNull(zoneConnection, "ERROR: A ZoneConection Component could not be found in the Parent hierarchy by ZoneConnectionOutlet in gameObject'" + gameObject.name + "'!");

        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        AIEnemy enemy = other.GetComponent<AIEnemy>();
        if (enemy && !zoneConnection.ContainsEnemy(enemy))
        {
            // If the zoneConnection doesn't contain the enemy, that means that the Enemy is exiting the conection through this outlet
            enemy.SetZoneController(outletZoneController);
        }
        else
        {
            Player player = other.GetComponentInParent<Player>();
            if (player && !zoneConnection.ContainsPlayer())
            {
                player.SetZoneController(outletZoneController, -transform.forward);
            }
        }
    }
    #endregion
}
