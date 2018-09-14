using UnityEngine;

public abstract class ScriptedAnimation : MonoBehaviour
{
    public delegate void VoidCallback();

    [SerializeField]
    private bool restoreObstructions = false;
    private VoidCallback endCallback;
    protected IObstructionsHandler obstructionsHandler;
    

    public void StartAnimation(VoidCallback voidCallback, IObstructionsHandler obstructionsHandler)
    {
        endCallback = voidCallback;
        this.obstructionsHandler = obstructionsHandler;
        StartAnimationInternal();
    }

    protected void OnAnimationFinished()
    {
        if (restoreObstructions)
        {
            obstructionsHandler.ShowObstructions();
        }
        obstructionsHandler = null;
        VoidCallback callback = endCallback;
        endCallback = null;
        callback();
    }

    protected abstract void StartAnimationInternal();
}
