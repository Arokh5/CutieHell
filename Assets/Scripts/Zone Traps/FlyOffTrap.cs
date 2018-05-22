using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyOffTrap : ZoneTrap
{
    private class FlyOffInfo
    {
        public AIEnemy enemy;
        public Vector3 motionVector;
        public float startDelay;
        public bool dead;
    }
    #region Fields
    private List<FlyOffInfo> flyOffInfos = new List<FlyOffInfo>();
    [SerializeField]
    private float flyStartDelayMax = 0.5f;
    [SerializeField]
    private float flyHeightMin = 15.0f;
    [SerializeField]
    private float flyHeightMax = 25.0f;
    [SerializeField]
    private float planeOffset = 5.0f;
    [SerializeField]
    private ParticleSystem particleSystemPrefab;

    private float elapsedTime;
    #endregion

    #region Protected Methods
    protected override void StartTrapEffect()
    {
        elapsedTime = 0.0f;
        List<AIEnemy> aiEnemies = zoneController.GetZoneEnemies();
        foreach (AIEnemy aiEnemy in aiEnemies)
        {
            FlyOffInfo info = new FlyOffInfo();
            info.enemy = aiEnemy;
            info.motionVector = new Vector3(Random.Range(-planeOffset, planeOffset), Random.Range(flyHeightMin, flyHeightMax), Random.Range(-planeOffset, planeOffset));
            info.startDelay = Random.Range(0.0f, flyStartDelayMax);
            info.dead = false;
            flyOffInfos.Add(info);
            aiEnemy.HitByZoneTrap();
            ParticlesManager.instance.LaunchParticleSystem(particleSystemPrefab, aiEnemy.transform.position, aiEnemy.transform.rotation);
        }
    }

    protected override void UpdateTrapEffect()
    {
        elapsedTime += Time.deltaTime;
        for (int i = 0; i < flyOffInfos.Count; ++i)
        {
            FlyOffInfo info = flyOffInfos[i];
            AIEnemy aiEnemy = info.enemy;
            if (elapsedTime > info.startDelay)
            {
                if (elapsedTime - info.startDelay < animationCooldownTime - flyStartDelayMax)
                    aiEnemy.transform.Translate(info.motionVector * Time.deltaTime / (animationCooldownTime - flyStartDelayMax), Space.World);
                else if (!info.dead)
                {
                    info.dead = true;
                    info.enemy.TakeDamage(info.enemy.baseHealth, AttackType.ZONE_TRAP);
                }

            }
        }
    }

    protected override void EndTrapEffect()
    {
        flyOffInfos.Clear();
    }
    #endregion
}
