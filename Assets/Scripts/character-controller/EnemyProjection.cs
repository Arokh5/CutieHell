using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjection : MonoBehaviour {

    private Transform transform;
    private GameObject trap;

    private float limitedPlacingDistance;
    // Use this for initialization
    void Start ()
    {
        transform = this.GetComponent<Transform>();
        trap = GameObject.Find("Trap_Seductive").transform.GetChild(0).GetChild(0).gameObject;

        limitedPlacingDistance = transform.parent.transform.parent.GetComponentInChildren<Projector>().orthographicSize * 0.7f;
        Debug.Log("Serializar");
    }
    // Update is called once per frame
    void Update ()
    {
        MoveEnemyProjection();
        RotateEnemyProjection();
    }
    #region Private Methods
    private void MoveEnemyProjection()
    { 
        if (InputManager.instance.GetLeftStickUp())
        {
            transform.localPosition += transform.forward * Time.deltaTime;
            if (Vector3.Distance(transform.position, trap.transform.position) > limitedPlacingDistance)
            {
                transform.localPosition -= transform.forward * Time.deltaTime;
            }
        }
        if (InputManager.instance.GetLeftStickDown())
        {
            transform.localPosition += -transform.forward * Time.deltaTime;
            if (Vector3.Distance(transform.position, trap.transform.position) > limitedPlacingDistance)
            {
                transform.localPosition += transform.forward * Time.deltaTime;
            }
        }
        if (InputManager.instance.GetLeftStickLeft())
        {
            transform.localPosition += -transform.right * Time.deltaTime;
            if (Vector3.Distance(transform.position, trap.transform.position) > limitedPlacingDistance)
            {
                transform.localPosition += transform.right * Time.deltaTime;
            }
        }
        if (InputManager.instance.GetLeftStickRight())
        {
            transform.localPosition += transform.right * Time.deltaTime;
            if (Vector3.Distance(transform.position, trap.transform.position) > limitedPlacingDistance)
            {
                transform.localPosition += -transform.right * Time.deltaTime;
            }
        }

    }

    private void RotateEnemyProjection()
    {
        transform.rotation = trap.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COlision detectada");
        transform.localPosition -= transform.forward * Time.deltaTime;
    }
    #endregion
}
