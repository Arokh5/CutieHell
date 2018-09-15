using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPooledParticleSystem : PooledParticleSystem
{
    #region Fields
    public float timeToReturnToPool = 0.0f;
    private float elapsedTime = 0.0f;
    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > timeToReturnToPool)
        {
            enabled = false;
            if (this.gameObject.GetComponent<BlackHole>() != null)
                GameManager.instance.GetPlayer1().SetIsBlackHoleOn(false);
            ReturnToPool();
        }
    }
    #endregion

    #region Public Methods
    public override void Restart()
    {
        elapsedTime = 0;
        enabled = true;
    }
    #endregion
}
