using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonInfo : MonoBehaviour {

    [Header("Canon Decal Fields")]
    public float limitedShootDistance;
    public float minimumShootDistance;
    public float decalMovementSpeed;


    [Header("Canon Fields")]
    public float canonWayOutPoint;
    public Transform canonTargetDecalGOTransform = null;

}
