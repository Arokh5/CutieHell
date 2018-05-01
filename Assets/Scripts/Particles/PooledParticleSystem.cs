using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledParticleSystem : MonoBehaviour
{
    #region Fields
    public float timeToReturnToPool = 0;
    [HideInInspector]
    public ParticleSystem prefabKey;
    private ParticleSystem particleSystem;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        UnityEngine.Assertions.Assert.IsNotNull(particleSystem, "ERROR: No ParticleSystem Component could be found by PooledParticlesystem in gameObject '" + gameObject.name + "'!");
    }
    #endregion

    #region Public Methods
    public void SetPrefabKey(ParticleSystem prefabKey)
    {
        this.prefabKey = prefabKey;
    }

    public void Restart()
    {
        StartCoroutine(TimedReturnToPool());
    }
    #endregion

    #region Private Methods
    private IEnumerator TimedReturnToPool()
    {
        yield return new WaitForSeconds(timeToReturnToPool);
        if (particleSystem)
            ParticlesManager.instance.ReturnParticleSystem(prefabKey, particleSystem);
        else
            Destroy(gameObject);
    }
    #endregion
}
