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
                if (weakTrap1.GetComponent<ConquerableElement>().GetActive() == true)
                {
                    weakTrap1.GetComponent<ConquerableElement>().SetBeingUsed(true);
                    updateEnemiesTarget(true);
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
                if (weakTrap1.GetComponent<ConquerableElement>().GetActive() == true)
                {
                    weakTrap1.GetComponent<ConquerableElement>().SetBeingUsed(false);
                    updateEnemiesTarget(false);
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
                if (weakTrap1.GetComponent<ConquerableElement>().GetActive() == true)
                {
                    weakTrap1.GetComponent<ConquerableElement>().SetBeingUsed(false);
                    weakTrap1.GetComponent<ConquerableElement>().SetConquered(true);
                    updateEnemiesTarget(false);
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
                if (weakTrap1.GetComponent<ConquerableElement>().GetActive() == true)
                {
                    weakTrap1.GetComponent<ConquerableElement>().SetConquered(false);
                }
            }
        }  
    }

    void updateEnemiesTarget(bool beingUsed)
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
