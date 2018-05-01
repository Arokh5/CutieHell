using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPooledParticleSystem : PooledParticleSystem
{
    #region Fields
    public float timeToReturnToPool = 0;
    #endregion

    #region Public Methods
    public override void Restart()
    {
        StartCoroutine(TimedReturnToPool());
    }
    #endregion

    #region Private Methods
    private IEnumerator TimedReturnToPool()
    {
        yield return new WaitForSeconds(timeToReturnToPool);
        ReturnToPool();
    }
    #endregion
}
