using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkersController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private ObjectiveMarker[] monumentIndicators = new ObjectiveMarker[3];
    #endregion

    #region Public Methods
    public void MonumentTargetted(int index)
    {
        if (index >= 0 && index < monumentIndicators.Length)
            monumentIndicators[index].MonumentTargetted();
    }

    public void MonumentConquered(int index)
    {
        if (index >= 0 && index < monumentIndicators.Length)
            monumentIndicators[index].MonumentTaken();
    }

    public void RequestFlash(int index)
    {
        if (index >= 0 && index < monumentIndicators.Length)
            monumentIndicators[index].RequestFlash();
    }
    #endregion
}
