using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuryZoneTrap : ZoneTrap
{
    private List<AIEnemy> aiEnemies = null;
    [SerializeField]
    private float buryDepth = 2.0f;

    #region Protected Methods
    protected override void StartTrapEffect()
    {
        aiEnemies = zoneController.GetZoneEnemies();
        foreach (AIEnemy aiEnemy in aiEnemies)
        {
            aiEnemy.HitByZoneTrap();
        }
    }

    protected override void UpdateTrapEffect()
    {
        foreach (AIEnemy aiEnemy in aiEnemies)
        {
            aiEnemy.transform.Translate(0, (-buryDepth / animationCooldownTime) * Time.deltaTime, 0, Space.World);
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
