using UnityEngine;

public class DeactivateOnTime : MonoBehaviour
{
    public float deactivationTime = -1;

    private void OnEnable()
    {
        Invoke("Disable", deactivationTime < 0 ? 0 : deactivationTime);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
