using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyManager : MonoBehaviour {

    public List<GameObject> butterfliesOptions;
    [SerializeField]
    private int butterfliesNumber;
    [SerializeField]
    private List<GameObject> butterflies;
    private List<Vector3> targets;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float yRatio;
    [SerializeField]
    private float detectionDistance;

    void Awake () {
        targets = new List<Vector3>();
        Vector3 newPos = Vector3.zero;
        float x = 0;
        float y = 0;
        float angle = 0;
        for (int i = butterflies.Count; butterfliesNumber > i; i++)
        {
            GameObject o = Instantiate(butterfliesOptions[Random.Range(0, butterfliesOptions.Count)]);
            angle = Random.Range(0, 360);
            x = Mathf.Sin(angle) * Random.Range(0, radius);
            y = Mathf.Cos(angle) * Random.Range(0, radius);
            newPos = this.transform.position + new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
            o.transform.position = newPos;
            o.GetComponent<Animator>().SetFloat("Speed", Random.Range(0.75f, 1.25f));
            o.transform.SetParent(this.transform);
            butterflies.Add(o);
            angle = Random.Range(0, 360);
            x = Mathf.Sin(angle) * Random.Range(0, radius);
            y = Mathf.Cos(angle) * Random.Range(0, radius);
            targets.Add(new Vector3(x, Random.Range(-radius, radius) * yRatio, y));
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

    private void NewTarget(int id)
    {
        float angle = Random.Range(0, 360);
        float x = Mathf.Sin(angle) * Random.Range(0,radius);
        float y = Mathf.Cos(angle) * Random.Range(0, radius);
        targets[id] = new Vector3(x, Random.Range(-radius, radius) * yRatio, y);
    }
}
