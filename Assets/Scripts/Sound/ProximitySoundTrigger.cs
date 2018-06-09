using UnityEngine;

public class ProximitySoundTrigger : SoundTrigger
{
    #region Fields
    public Transform reference;
    public float triggerDistance;
    #endregion

    #region MonoBehaviour Methods
    private void Update()
    {
        if (Vector3.SqrMagnitude(reference.position - transform.position) <= triggerDistance * triggerDistance)
            TriggerOn();
        else
            TriggerOff();
    }
    #endregion
}
