using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HighSpeedZone : MonoBehaviour
{
    #region MonoBehaviour Methods
    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        AIEnemy enemy = other.GetComponent<AIEnemy>();
        if (enemy)
            enemy.SetInHighSpeedZone(true);
    }

    private void OnTriggerExit(Collider other)
    {
        AIEnemy enemy = other.GetComponent<AIEnemy>();
        if (enemy)
            enemy.SetInHighSpeedZone(false);
    }
    #endregion
}
