using UnityEngine;

public abstract class PooledParticleSystem : MonoBehaviour
{
    #region Fields
    protected ParticleSystem prefabKey;
    protected ParticleSystem particleSystemInstance;
    #endregion

    #region Public Methods
    public void SetPrefabKey(ParticleSystem prefabKey)
    {
        this.prefabKey = prefabKey;
    }

    public abstract void Restart();

    public void ReturnToPool()
    {
        if (!particleSystemInstance)
        {
            particleSystemInstance = GetComponent<ParticleSystem>();
            UnityEngine.Assertions.Assert.IsNotNull(particleSystemInstance, "ERROR: No ParticleSystem Component could be found by PooledParticlesystem in gameObject '" + gameObject.name + "'!");
        }

        if (particleSystemInstance)
            ParticlesManager.instance.ReturnParticleSystem(prefabKey, particleSystemInstance);
        else
            Destroy(gameObject);
    }
    #endregion

    
}
