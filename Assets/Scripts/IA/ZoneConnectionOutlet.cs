using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneConnectionOutlet : MonoBehaviour
{
    #region Fields
    private ZoneConnection zoneConnection;
    public AIZoneController outletZoneController;
    [Tooltip("Set to true to not change the zoneController of Enemies when they exit through this outlet")]
    public bool ignoreEnemies;
    [Tooltip("Set to true to not change the zoneController of the Player when it exit through this outlet")]
    public bool ignorePlayer;

    private const float knockbackTargetDistance = 4.0f;
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
        AIEnemy enemy = null;
        if (!ignoreEnemies)
            enemy = other.GetComponent<AIEnemy>();

        if (enemy && !zoneConnection.ContainsEnemy(enemy))
        {
            // If the zoneConnection doesn't contain the enemy, that means that the Enemy is exiting the conection through this outlet
            enemy.SetZoneController(outletZoneController);
        }
        else
        {
            Player player = null;
            if (!ignorePlayer)
                player = other.GetComponentInParent<Player>();

            if (player && !zoneConnection.ContainsPlayer())
            {
                Vector3 targetPos = transform.position - knockbackTargetDistance * transform.forward;
                player.SetZoneController(outletZoneController, targetPos - player.transform.position);
            }
        }
    }
    #endregion
}
