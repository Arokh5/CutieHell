using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackDecalSize : MonoBehaviour {

    public float size = 3;
    public float scale = 0.1f;
    public Projector projector;
    public GameObject explosion;

	void OnEnable ()
    {
        UnityEngine.Assertions.Assert.IsNotNull(projector, "ERROR: A Projector Component could not be found in GameObject " + gameObject.name);
        UnityEngine.Assertions.Assert.IsNotNull(explosion, "ERROR: A GameObject could not be found in GameObject " + gameObject.name);
    }

    public void UpdateThis()
    {
        projector.orthographicSize = size;
        explosion.transform.localScale = new Vector3(scale, scale, scale);
    }

}
