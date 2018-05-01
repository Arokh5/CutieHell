using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    #region Fields
    public static ParticlesManager instance;

    public Transform pooledParticles;
    public Transform activeParticles;

    private Dictionary<ParticleSystem, ObjectPool<ParticleSystem>> particlesPool = new Dictionary<ParticleSystem, ObjectPool<ParticleSystem>>();
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    #endregion

    #region Public Methods
    public ParticleSystem LaunchParticleSystem(ParticleSystem prefab, Vector3 position, Quaternion rotation)
    {
        ObjectPool<ParticleSystem> pool;
        if (particlesPool.ContainsKey(prefab))
        {
            pool = particlesPool[prefab];
        }
        else
        {
            pool = new ObjectPool<ParticleSystem>(prefab, pooledParticles);
            particlesPool.Add(prefab, pool);
        }

        ParticleSystem particleSystem = pool.GetObject(activeParticles, position, rotation, true, true);

        PooledParticleSystem poolComponent = particleSystem.GetComponent<PooledParticleSystem>();
        if (!poolComponent)
            poolComponent = particleSystem.gameObject.AddComponent<PooledParticleSystem>();

        poolComponent.SetPrefabKey(prefab);
        poolComponent.Restart();

        particleSystem.Play();
        return particleSystem;
    }

    public void ReturnParticleSystem(ParticleSystem prefabKey, ParticleSystem particleSystem)
    {
        particleSystem.Stop();

        if (particlesPool.ContainsKey(prefabKey))
        {
            ObjectPool<ParticleSystem> pool = particlesPool[prefabKey];
            pool.ReturnToPool(particleSystem);
        }
        else
        {
            Debug.LogWarning("WARNING: Attempted to return a ParticleSystem that didn't come from the ParticlesManager. The ParticleSystem will be Destroyed.");
            Destroy(particleSystem.gameObject);
        }
    }
    #endregion

}
