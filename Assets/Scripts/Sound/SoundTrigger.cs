using UnityEngine;

[System.Serializable]
public abstract class SoundTrigger: MonoBehaviour
{
    #region Fields
    private SoundEmitter soundEmitter;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        soundEmitter = GetComponentInParent<SoundEmitter>();
        UnityEngine.Assertions.Assert.IsNotNull(soundEmitter, "ERROR: SoundTrigger in gameObject '" + gameObject.name + "' could not find a SoundEmitter in its parent hierarchy!");
    }
    #endregion

    #region Protected Methods
    protected void TriggerOn()
    {
        soundEmitter.TriggerOn();
    }

    protected void TriggerOff()
    {
        soundEmitter.TriggerOff();
    }
    #endregion
}
