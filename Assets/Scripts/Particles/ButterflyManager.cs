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

	void OnEnable () {
        targets = new List<Vector3>();
        for(int i = butterflies.Count; butterfliesNumber > butterflies.Count; i++)
        {
            GameObject o = Instantiate(butterfliesOptions[Random.Range(0, 5)]);
            o.GetComponent<Animator>().SetFloat("Speed", Random.Range(0.75f, 1.25f));
            //o.transform.SetParent(this.transform);
            butterflies.Add(o);
        }
        Vector3 newPos = Vector3.zero;
        int k = butterflies.Count;
        for (int i = 0; i < k; i++)
        {
            targets.Add(this.transform.position + new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius)));
            newPos = this.transform.position + new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius));
            butterflies[i].transform.position = newPos;
            butterflies[i].transform.LookAt(targets[i]);
        }
	}
	
	void Update () {
        int k = butterflies.Count;

        for (int i = 0; i < k; i++)
        {
            if(Vector3.SqrMagnitude(butterflies[i].transform.localPosition - (targets[i])) < 0.5f)
            {
                NewTarget(i);
            }
            butterflies[i].transform.Translate(Vector3.forward * 1.5f * Time.deltaTime);
            butterflies[i].transform.LookAt(targets[i]);
        }

	}

    private void NewTarget(int id)
    {
        targets[id] = this.transform.position + new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius));
        butterflies[id].transform.LookAt(targets[id]);
    }
}
