using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraculaController : MonoBehaviour {


    [Header("Speed Variabes")]
    public float maxSpeed = 10;
    public float acceleration = 50;

    private Rigidbody rb;
    private Vector3 speedDirection;

	void Start () 
    {
        rb = this.GetComponent<Rigidbody>();
        speedDirection = Vector3.zero;
	}

    private void FixedUpdate() 
    {
        speedDirection = Vector3.zero;

        if (InputManager.instance.GetLeftStickUp()) {
            speedDirection += new Vector3(0.0f, 0.0f, 0.5f);
        }
        if (InputManager.instance.GetLeftStickDown()) {
            speedDirection += new Vector3(0.0f, 0.0f, -0.1f);
        }
        if (InputManager.instance.GetLeftStickLeft()) {
            speedDirection += new Vector3(-0.5f, 0.0f, 0.0f);
        }
        if (InputManager.instance.GetLeftStickRight()) {
            speedDirection += new Vector3(0.5f, 0.0f, 0.0f);
        }

        if (speedDirection.magnitude > 0.0f) 
        {
            rb.drag = 0.0f;
        } 
        else 
        {
            rb.drag = 10.0f;
        }

        Vector3 temp = speedDirection * acceleration;

        rb.AddRelativeForce(speedDirection * acceleration, ForceMode.Acceleration);

        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
