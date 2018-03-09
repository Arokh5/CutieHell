using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneConnection : MonoBehaviour {

    #region Fields
    private uint zoneConnetionID;
    private AIZoneController zone1;
    private AIZoneController zone2;
    #endregion

    #region Public Methods
    // Called by one of two Zonecontroller when its Monument gets conquered
    public void Open()
    {
        Debug.LogError("NOT IMPLEMENTED: ZoneConnection::Open");
    }

    // Called by one of two Zonecontroller when its Monument gets repaired
    public void Close()
    {
        Debug.LogError("NOT IMPLEMENTED: ZoneConnection::Close");
    }
    #endregion
}
