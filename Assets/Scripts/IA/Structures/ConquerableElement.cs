using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquerableElement : MonoBehaviour {

    private bool active; //exists on the battlefield or not
    private string area;

    public bool conquered;
    public bool beingUsed;

    public string type;

    // Use this for initialization
    void Start () {
        if (type != null)
        {
            active = true;
            beingUsed = false; //only trap matters
            conquered = false;
        }
        area = transform.parent.name;
	}

    public void SetConquered(bool isConquered)
    {
        bool playerCurrentlyUsingAnyTrap = false;
        conquered = isConquered;
        if (type == "conquerableDefense")
        {
            GameObject.Find("AreasLinker").GetComponent<AreasLinker>().SetOpened(conquered); //TODO find expensive
        }

        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).name.Contains("Trap"))
            {
                if (playerCurrentlyUsingAnyTrap = transform.parent.transform.GetChild(i).GetComponent<ConquerableElement>().GetBeingUsed() == true)
                {
                    return;
                }
            }
        }

        if(playerCurrentlyUsingAnyTrap == false)
        {
            GameObject affectedEnemies = GameObject.Find("SlimesA1_"); //TODO find expensive an only slimes
            for (int i = 0; i < affectedEnemies.transform.childCount; i++)
            {
                affectedEnemies.transform.GetChild(i).GetComponent<EnemyBehaviour>().UpdateTarget(GameObject.Find("ConquerableStructure2").transform);  //TODO hardcoded
            }
        }
       
    }

    public bool GetConquered()
    {
        return conquered;
    }

    public void SetActive(bool isActive)
    {
        active = isActive;
    }

    public bool GetActive()
    {
        return active;
    }

    public void SetBeingUsed(bool isBeingUsed)
    {
        beingUsed = isBeingUsed;

        GameObject affectedEnemies = GameObject.Find("SlimesA1_"); //TODO find expensive an only slimes
        for (int i = 0; i < affectedEnemies.transform.childCount; i++)
        {
            if (beingUsed == true)
            {
                affectedEnemies.transform.GetChild(i).GetComponent<EnemyBehaviour>().UpdateTarget(transform);  //TODO hardcoded
            }
            else
            {
                affectedEnemies.transform.GetChild(i).GetComponent<EnemyBehaviour>().UpdateTarget(transform.parent.Find("ConquerableStructure1").transform);
            }
                
        }
    }

    public bool GetBeingUsed()
    {
        return beingUsed;
    }

    public string GetArea()
    {
        return area;
    }
}
