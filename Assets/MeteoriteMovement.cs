using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteMovement : MonoBehaviour {

    public float speed = 8.0f;

	void Update () {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
