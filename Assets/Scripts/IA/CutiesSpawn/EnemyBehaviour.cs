using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class EnemyBehaviour : MonoBehaviour {

    [SerializeField]
    private string m_AISubTeamID;

    public float attackRange = 5;
    public float dps = 0.5f;

    private Transform target;
    private ConquerableBuilding cBuilding;
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
        Transform weakTrap = areaParent.transform.Find("weakTrapArea1_1");
        if (weakTrap)
        {
            ConquerableElement cElement = weakTrap.GetComponent<ConquerableElement>();
            if (cElement && cElement.GetBeingUsed())
            {
                target = weakTrap;
            }
            else
            {
                cBuilding = weakTrap.GetComponent<ConquerableBuilding>();
                if (cBuilding && cBuilding.GetBeingUsed())
                {
                    target = weakTrap;
                }
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
        if(target) 
        {
            Vector3 destination = new Vector3(target.position.x, this.transform.position.y, target.position.z); //The Y will be the own to avoid problems with mesh and not valid y values
            agent.SetDestination(destination);

            if (cBuilding && Vector3.Distance(this.transform.position, target.position) < attackRange)
            {
                cBuilding.TakeDamage(dps * Time.deltaTime);
            }
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
