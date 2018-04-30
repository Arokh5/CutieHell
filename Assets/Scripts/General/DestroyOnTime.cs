using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float timeToDestroy = 0;

	private void Start () {
        Destroy(gameObject, timeToDestroy);
	}
}
