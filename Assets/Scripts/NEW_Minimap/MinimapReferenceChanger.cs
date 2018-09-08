using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapReferenceChanger : MonoBehaviour, IZoneTakenListener
{
    #region Fields
    [SerializeField]
    [Tooltip("The AIZoneController to listen to")]
    private AIZoneController referenceZone;
    [SerializeField]
    [Tooltip("The minimap whose worldReference index should be changed")]
    private MinimapController minimapController;
    [SerializeField]
    [Tooltip("The worldReference index that should be applied to the Minimap Controller")]
    private int targetIndex = -1;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(referenceZone, "ERROR: Reference Zone (AIZoneController) not assigned for MinimapReferenceChanger script in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(minimapController, "ERROR: Minimap Controller (MinimapController) not assigned for MinimapReferenceChanger script in GameObject " + gameObject.name);
        if (targetIndex == -1)
        {
            Debug.LogError("ERROR: Target Index (int) left at the default value of -1 in MinimapReferenceChanger script in GameObject " + gameObject.name);
        }
    }

    private void Start()
    {
        referenceZone.AddIZoneTakenListener(this);
    }

    private void OnDestroy()
    {
        referenceZone.RemoveIZoneTakenListener(this);
    }
    #endregion

    #region Public Methods
    public void OnZoneTaken()
    {
        minimapController.ChangeMinimapReference(targetIndex);
    }
    #endregion
}
