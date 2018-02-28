using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapsManagement : MonoBehaviour {

    [SerializeField]
    public GameObject area1;
    [SerializeField]
    public GameObject area2;

    public Transform trapTransform;

    // Use this for initialization
    void Start()
    { 
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UnlockAreaTrap(1, 1);
            UnlockAreaTrap(2, 1);
            UnlockAreaTrap(3, 1);
        }
    }

    private void clear()
    {
        //if(GameObject.Find("trap"+"Area1"+ 1.ToString()))
        //{
        //    GameObject trapToDelete = GameObject.Find("trap" + "Area1" + 1.ToString());
        //    Destroy(trapToDelete);
        //}
    }

    private void UnlockAreaTrap(int trapNumber,int areaNumber)
    {
        Vector3 trapPosition = new Vector3(0,0,0);

        switch (trapNumber)
        {
            case 1:
                if (areaNumber == 1)
                {
                    trapPosition.x = 95;
                    trapPosition.y = -30;
                    trapPosition.z = 85;
                } 
                break;
           case 2:
                if(areaNumber == 1)
                {
                    trapPosition.x = 60;
                    trapPosition.y = -30;
                    trapPosition.z = 93;  
                }
                break;
            case 3:
                if (areaNumber == 1)
                {
                    trapPosition.x = 25;
                    trapPosition.y = -30;
                    trapPosition.z = 100;
                }
                break;
        }

        trapTransform = Instantiate(trapTransform, trapPosition, Quaternion.identity);
        trapTransform.parent = area1.transform;  

        if(areaNumber == 1)
        {
            trapTransform.name = "weakTrap" + "Area1_" + trapNumber.ToString();
        }
        else if(areaNumber == 2)
        {
            trapTransform.name = "weakTrap" + "Area2_" + trapNumber.ToString();
        }
        
    }
}
