using UnityEngine;

public abstract class ScriptedAnimation : MonoBehaviour
{
    public delegate void VoidCallback();
    private VoidCallback endCallback;

    public void StartAnimation(VoidCallback voidCallback)
    {
        endCallback = voidCallback;
        StartAnimationInternal();
    }

    protected void OnAnimationFinished()
    {
        VoidCallback callback = endCallback;
        endCallback = null;
        callback();
    }

    protected abstract void StartAnimationInternal();
}
