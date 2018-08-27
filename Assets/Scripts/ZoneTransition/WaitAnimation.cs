using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimation : ScriptedAnimation
{
    #region Fields
    [SerializeField]
    private float waitTime = 1.0f;

    private bool animating;
    private float elapsedTime = 0.0f;
    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        if (animating)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > waitTime)
                EndAnimation();

        }
    }

    private void OnValidate()
    {
        if (waitTime < 0.0f)
            waitTime = 0.0f;
    }
    #endregion

    #region Protected Methods
    protected override void StartAnimationInternal()
    {
        animating = true;
        elapsedTime = 0.0f;
    }
    #endregion

    #region Private Methods
    private void EndAnimation()
    {
        animating = false;
        OnAnimationFinished();
    }
    #endregion
}
