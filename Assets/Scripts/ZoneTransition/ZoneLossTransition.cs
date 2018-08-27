using UnityEngine;

public class ZoneLossTransition : MonoBehaviour
{
    public delegate void VoidCallback();
    #region Fields

    private bool inTransition = false;
    public float elapsedTime;
    private VoidCallback endCallback = null;
    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        if (inTransition)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 2.0f)
                OnTransitionFinished();
        }
    }
    #endregion

    #region Public Methods
    public void StartTransition(VoidCallback voidCallback)
    {
        endCallback = voidCallback;
        elapsedTime = 0.0f;
        inTransition = true;
    }
    #endregion

    #region Private Methods
    private void OnTransitionFinished()
    {
        inTransition = false;
        VoidCallback callback = endCallback;
        endCallback = null;
        callback();
    }
    #endregion
}
