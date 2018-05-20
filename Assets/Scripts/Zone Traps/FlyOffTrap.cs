using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyOffTrap : ZoneTrap
{
    #region Fields
    private List<AIEnemy> aiEnemies = null;
    private List<Vector3> directions = null;
    [SerializeField]
    private float flyDistance = 20.0f;
    [SerializeField]
    private float planeOffset = 5.0f;
    #endregion

    #region Protected Methods
    protected override void StartTrapEffect()
    {
        aiEnemies =  new List<AIEnemy>(zoneController.GetZoneEnemies());
        directions = new List<Vector3>();
        foreach (AIEnemy aiEnemy in aiEnemies)
        {
            aiEnemy.HitByZoneTrap();
            Vector3 direction = new Vector3(Random.Range(-planeOffset, planeOffset), flyDistance, Random.Range(-planeOffset, planeOffset));
            directions.Add(direction);
        }
    }

    protected override void UpdateTrapEffect()
    {
        for (int i = 0; i < aiEnemies.Count; ++i)
        {
            AIEnemy aiEnemy = aiEnemies[i];
            aiEnemy.transform.Translate(directions[i] * Time.deltaTime / animationCooldownTime, Space.World);
        }
    }

    protected override void EndTrapEffect()
    {
        foreach (AIEnemy aiEnemy in aiEnemies)
        {
            aiEnemy.TakeDamage(aiEnemy.baseHealth, AttackType.ZONE_TRAP);
        }
        aiEnemies = null;
    }
    #endregion
}
