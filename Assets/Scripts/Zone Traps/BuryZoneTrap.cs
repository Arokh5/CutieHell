using System.Collections.Generic;
using UnityEngine;

public class BuryZoneTrap : ZoneTrap
{
    [System.Serializable]
    private class EnemyTypeParticlesPrefab
    {
        public EnemyType type;
        public ParticleSystem particlesPrefab;
    }

    #region Fields
    private List<AIEnemy> aiEnemies = null;
    [SerializeField]
    private float buryDepth = 2.0f;
    [SerializeField]
    private EnemyTypeParticlesPrefab[] particleSystems;
    private Dictionary<EnemyType, ParticleSystem> particles;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        particles = new Dictionary<EnemyType, ParticleSystem>();
        foreach (EnemyTypeParticlesPrefab prefab in particleSystems)
        {
            particles.Add(prefab.type, prefab.particlesPrefab);
        }
    }
    #endregion

    #region Protected Methods
    protected override void StartTrapEffect()
    {
        aiEnemies = new List<AIEnemy>(zoneController.GetZoneEnemies());
        foreach (AIEnemy aiEnemy in aiEnemies)
        {
            aiEnemy.HitByZoneTrap();
            ParticleSystem prefab = particles[aiEnemy.enemyType];
            ParticlesManager.instance.LaunchParticleSystem(prefab, aiEnemy.transform.position, aiEnemy.transform.rotation);
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
            //aiEnemy.TakeDamage(aiEnemy.baseHealth, AttackType.ZONE_TRAP);
        }
        aiEnemies = null;
    }
    #endregion
}
