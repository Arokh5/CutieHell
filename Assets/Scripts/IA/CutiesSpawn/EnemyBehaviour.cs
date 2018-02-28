using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class EnemyBehaviour : MonoBehaviour {

    [SerializeField]
    private string m_AISubTeamID;

    private Transform target;
    private String area;

    NavMeshAgent agent;
    List<GameObject> spawnedElements = new List<GameObject>();

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        //this.transform.parent = GameObject.Find("Area1").transform;
    }

    public void InitializeTarget()
    {
        GameObject areaParent = transform.parent.transform.parent.gameObject;
        target = areaParent.GetComponent<GameAreaManager>().defensePoint.transform;

        //TODO manage own area defense point being conquered
        if (areaParent.transform.Find("weakTrapArea1_1") != null)
        {
            if (areaParent.transform.Find("weakTrapArea1_1").GetComponent<ConquerableElement>().GetBeingUsed())
            {
                target = areaParent.transform.Find("weakTrapArea1_1").transform;
            }
        }
    }


    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null) 
        {
            Vector3 destination = new Vector3(target.position.x, this.transform.position.y, target.position.z); //The Y will be the own to avoid problems with mesh and not valid y values
            agent.SetDestination(destination);
        }
        
    }

    public void Clustering()
    {
        GameObject element;
        for (int i = 0; i < spawnedElements.Count; i++)
        {
            element = spawnedElements[i];
            if (this.m_AISubTeamID.Equals(element.GetComponentInChildren<EnemyBehaviour>().getAISubTeamID()))
            {
                if (this.transform != element.transform && Vector3.Distance(this.transform.position, element.transform.position) > 4 && Vector3.Distance(this.transform.position, element.transform.position) < 7)
                {
                    transform.position = Vector3.MoveTowards(transform.position, element.transform.position, 0.01f);
                }
            }
        }
    }

    public void SetArea(String paramArea)
    {
        area = paramArea;
    }
    public String GetArea()
    {
        return area;
    }

    public string getAISubTeamID()
    {
        return m_AISubTeamID;
    }

    public void setAISubTeamID(string subTeamID)
    {
        m_AISubTeamID = subTeamID;
    }
}
