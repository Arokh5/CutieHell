using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyManager : MonoBehaviour {

    public List<GameObject> butterfliesOptions;
    [SerializeField]
    private int butterfliesNumber;
    private List<GameObject> butterflies;
    private List<Vector3> targets;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float minRadius;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float yRatio;
    [SerializeField]
    private float detectionDistance;
    [SerializeField]
    private bool circularMove;

    void Awake () {
        targets = new List<Vector3>();
        butterflies = new List<GameObject>();
        //Vector3 newPos = Vector3.zero;
        float x = 0;
        float y = 0;
        float angle = 0;
        for (int i = butterflies.Count; butterfliesNumber > i; i++)
        {
            GameObject o = Instantiate(butterfliesOptions[Random.Range(0, butterfliesOptions.Count)]);
            //angle = Random.Range(0, Mathf.PI * 2);
            //x = Mathf.Sin(angle) * Random.Range(0, 0.5f);
            //y = Mathf.Cos(angle) * Random.Range(0, 0.5f);
            //newPos = this.transform.position + new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            //o.transform.position = newPos;
            o.GetComponent<Animator>().SetFloat("Speed", Random.Range(0.75f, 1.25f));
            o.transform.SetParent(this.transform);
            butterflies.Add(o);
            angle = Random.Range(0, Mathf.PI * 2);
            x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
            y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
            targets.Add(new Vector3(x, Random.Range(-radius, radius) * yRatio, y));
        }
	}
    private void OnEnable()
    {
        Vector3 newPos = Vector3.zero;
        float x = 0;
        float y = 0;
        float angle = 0;
        for (int i = 0; butterfliesNumber > i; i++)
        {
            angle = Random.Range(0, Mathf.PI * 2);
            x = Mathf.Sin(angle) * Random.Range(0, 0.5f);
            y = Mathf.Cos(angle) * Random.Range(0, 0.5f);
            newPos = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            butterflies[i].transform.localPosition = newPos;
        }
    }

    void Update () {
        int k = butterflies.Count;
        Vector3 parentRotation = this.transform.parent.transform.rotation.eulerAngles;
        for (int i = 0; i < k; i++)
        {
            Vector3 target = targets[i];
            target = Quaternion.Euler(0, parentRotation.y, 0) * target;

            if (Vector3.SqrMagnitude(butterflies[i].transform.position - (this.transform.position + target)) < detectionDistance)
            {
                NewTarget(i);
            }
            butterflies[i].transform.LookAt(this.transform.position + target);
            butterflies[i].transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }

	}

    private void Reposition()
    {
        for(int i = 0; i < butterfliesNumber; i++)
        {

        }
    }

    private void NewTarget(int id)
    {
        if (!circularMove)
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
            float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
            targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
        }
        else
        {
            float initialRotation = Vector3.Angle(targets[id], Vector3.forward);
            if (targets[id].x < 0.0f) initialRotation = 360 - initialRotation;
            if (initialRotation < 45)
            {
                float angle = Random.Range(Mathf.PI / 4, Mathf.PI / 2);
                float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
                float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
                targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            }
            else if (initialRotation < 90)
            {
                float angle = Random.Range(Mathf.PI / 2, Mathf.PI / 4 * 3);
                float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
                float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
                targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            }
            else if (initialRotation < 135)
            {
                float angle = Random.Range(Mathf.PI / 4 * 3, Mathf.PI);
                float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
                float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
                targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            }
            else if (initialRotation < 180)
            {
                float angle = Random.Range(Mathf.PI, Mathf.PI / 4 * 5);
                float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
                float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
                targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            }
            else if (initialRotation < 225)
            {
                float angle = Random.Range(Mathf.PI / 4 * 5, Mathf.PI / 4 * 6);
                float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
                float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
                targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            }
            else if (initialRotation < 270)
            {
                float angle = Random.Range(Mathf.PI / 4 * 6, Mathf.PI / 4 * 7);
                float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
                float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
                targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            }
            else if (initialRotation < 315)
            {
                float angle = Random.Range(Mathf.PI / 4 * 7, Mathf.PI * 2);
                float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
                float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
                targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            }
            else if(initialRotation < 360)
            {
                float angle = Random.Range(Mathf.PI * 2, Mathf.PI / 4 * 9);
                float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
                float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
                targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            }
            //if (targets[id].x >= 0.0f && targets[id].z >= 0.0f)
            //{
            //    float angle = Random.Range(Mathf.PI / 2, Mathf.PI);
            //    float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
            //    float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
            //    targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            //}
            //else if (targets[id].x > 0.0f && targets[id].z <= 0.0f)
            //{
            //    float angle = Random.Range(Mathf.PI, Mathf.PI / 2 * 3);
            //    float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
            //    float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
            //    targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            //}
            //else if (targets[id].x < 0.0f && targets[id].z < 0.0f)
            //{
            //    float angle = Random.Range(Mathf.PI / 2 * 3, Mathf.PI * 2);
            //    float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
            //    float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
            //    targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            //}
            //else if (targets[id].x < 0.0f && targets[id].z > 0.0f)
            //{
            //    float angle = Random.Range(0, Mathf.PI / 2);
            //    float x = Mathf.Sin(angle) * Random.Range(minRadius, radius);
            //    float y = Mathf.Cos(angle) * Random.Range(minRadius, radius);
            //    targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            //}
        }
    }
}
