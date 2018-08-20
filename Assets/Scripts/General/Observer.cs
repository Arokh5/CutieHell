using UnityEngine;

public abstract class Observer : MonoBehaviour
{
    #region Private methods
    public abstract void OnNotify();
	#endregion
}
