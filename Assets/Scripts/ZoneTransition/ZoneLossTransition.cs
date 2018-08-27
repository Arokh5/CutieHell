using UnityEngine;

public class ZoneLossTransition : MonoBehaviour
{
    public delegate void VoidCallback();
    #region Fields
    [SerializeField]
    private ScriptedAnimation[] scriptedAnimations;

    private bool inTransition = false;
    private int currentAnimationIndex = -1;
    private VoidCallback endCallback = null;
    #endregion

    #region MonoBehaviour Methods

    #endregion

    #region Public Methods
    public void StartTransition(VoidCallback voidCallback)
    {
        endCallback = voidCallback;
        StartAnimationChain();
        inTransition = true;
    }
    #endregion

    #region Private Methods
    private void StartAnimationChain()
    {
        Debug.Log("Starting chain");
        currentAnimationIndex = -1;
        if (scriptedAnimations != null && scriptedAnimations.Length > 0)
        {
            StartNextAnimation();
        }
        else
        {
            OnTransitionFinished();
        }
    }

    private void StartNextAnimation()
    {
        ++currentAnimationIndex;

        if (currentAnimationIndex < scriptedAnimations.Length)
        {
            Debug.Log("Moving to element " + currentAnimationIndex);
            scriptedAnimations[currentAnimationIndex].StartAnimation(StartNextAnimation);
        }
        else
        {
            OnTransitionFinished();
        }
    }

    private void OnTransitionFinished()
    {
        Debug.Log("Finishing chain");
        inTransition = false;
        VoidCallback callback = endCallback;
        endCallback = null;
        callback();
    }
    #endregion
}
