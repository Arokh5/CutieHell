using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenAnimation : MonoBehaviour
{
    #region Fields
    [Header("Elements setup")]
    [SerializeField]
    private Transform fogTransform;

    [Header("Configuration")]
    [SerializeField]
    private float fogRotationSpeed;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(fogTransform, "ERROR: Fog Transform (Transform) not assigned for TitleScreenAnimation script in GameObject '" + gameObject.name + "'!");
    }

    private void Update()
    {
        fogTransform.Rotate(fogRotationSpeed * Time.deltaTime * Vector3.up);
    }
    #endregion
}
