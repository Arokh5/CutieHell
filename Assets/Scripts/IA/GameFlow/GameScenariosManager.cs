using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScenariosManager : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    public GameObject area1;
    [SerializeField]
    public GameObject area2;

    private Transform weakTrap1;

    // Update is called once per frame
    void Update()
    {
        //Defense Point Conquered
        if (Input.GetKeyDown(KeyCode.C))
        {
            area1.transform.Find("ConquerableStructure1").GetComponent<ConquerableElement>().SetConquered(!transform.Find("Area1").Find("ConquerableStructure1").GetComponent<ConquerableElement>().GetConquered()); //TODO substitute the hardcoded
        }

        //Use trap
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (area1.transform.Find("weakTrapArea1_1") != null)
            {
                if (weakTrap1 == null)
                {
                    weakTrap1 = area1.transform.Find("weakTrapArea1_1");
                }
                ConquerableElement cElement = weakTrap1.GetComponent<ConquerableElement>();
                if (cElement && cElement.GetActive() == true)
                {
                    cElement.SetBeingUsed(true);
                    updateEnemiesTarget(true);
                }
                else
                {
                    ConquerableBuilding cBuilding = weakTrap1.GetComponent<ConquerableBuilding>();
                    if (cBuilding)
                    {
                        cBuilding.gameScenariosManager = this;
                        cBuilding.SetBeingUsed(true);
                        updateEnemiesTarget(true);
                    }
                }
            }
        }

        //Stop using trap
        if(Input.GetKeyDown(KeyCode.D))
        {
            if (area1.transform.Find("weakTrapArea1_1") != null)
            {
                if (weakTrap1 == null)
                {
                    weakTrap1 = area1.transform.Find("weakTrapArea1_1");
                }
                ConquerableElement cElement = weakTrap1.GetComponent<ConquerableElement>();
                if (cElement && cElement.GetActive() == true)
                {
                    cElement.SetBeingUsed(false);
                    updateEnemiesTarget(false);
                }
                else
                {
                    ConquerableBuilding cBuilding = weakTrap1.GetComponent<ConquerableBuilding>();
                    if (cBuilding)
                    {
                        cBuilding.SetBeingUsed(false);
                        updateEnemiesTarget(false);
                    }
                }
            }
        }

        //Trap Conquered
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (area1.transform.Find("weakTrapArea1_1") != null)
            {
                if (weakTrap1 == null)
                {
                    weakTrap1 = area1.transform.Find("weakTrapArea1_1");
                }
                ConquerableElement cElement = weakTrap1.GetComponent<ConquerableElement>();
                if (cElement && cElement.GetActive() == true)
                {
                    cElement.SetBeingUsed(false);
                    cElement.SetConquered(true);
                    updateEnemiesTarget(false);
                }
                else
                {
                    ConquerableBuilding cBuilding = weakTrap1.GetComponent<ConquerableBuilding>();
                    if (cBuilding)
                    {
                        cBuilding.SetBeingUsed(false);
                        cBuilding.SetConquered(true);
                        updateEnemiesTarget(false);
                    }
                }
            }
        }

        //Trap repaired
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (area1.transform.Find("weakTrapArea1_1") != null)
            {
                if (weakTrap1 == null)
                {
                    weakTrap1 = area1.transform.Find("weakTrapArea1_1");
                }

                ConquerableElement cElement = weakTrap1.GetComponent<ConquerableElement>();
                if (cElement && cElement.GetActive() == true)
                {
                    cElement.SetConquered(false);
                }
                else
                {
                    ConquerableBuilding cBuilding = weakTrap1.GetComponent<ConquerableBuilding>();
                    if (cBuilding)
                    {
                        cBuilding.SetConquered(false);
                    }
                }
            }
        }  
    }

    public void updateEnemiesTarget(bool beingUsed)
    {
        List<GameObject> affectedEnemies = new List<GameObject>(); //Improve memory management
        affectedEnemies.Add(GameObject.Find("SlimesA1_"));
        affectedEnemies.Add(GameObject.Find("FightersA1_"));
        for (int j = 0; j < affectedEnemies.Count; j++)
        {
            for (int i = 0; i < affectedEnemies[j].transform.childCount; i++)
            {
                if (beingUsed == true)
                {
                    affectedEnemies[j].transform.GetChild(i).GetComponent<EnemyBehaviour>().UpdateTarget(weakTrap1);
                }
                else
                {
                    affectedEnemies[j].transform.GetChild(i).GetComponent<EnemyBehaviour>().UpdateTarget(area1.transform.Find("ConquerableStructure1").transform); //Area1
                }

            }
        }
    }
}
