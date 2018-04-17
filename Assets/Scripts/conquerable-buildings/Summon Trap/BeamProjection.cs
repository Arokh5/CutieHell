using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamProjection : MonoBehaviour {

    public Transform projectionTrapCastPoint;
    public Transform enemyProjectionTargetPoint;
    LineRenderer beamLine;
	// Use this for initialization
	void Start () {
        beamLine = GetComponent<LineRenderer>();
        beamLine.SetWidth(.2f, .2f);
	}
	
	// Update is called once per frame
	void Update () {
        beamLine.SetPosition(0, projectionTrapCastPoint.position);
        beamLine.SetPosition(1, enemyProjectionTargetPoint.position);
    }
}
