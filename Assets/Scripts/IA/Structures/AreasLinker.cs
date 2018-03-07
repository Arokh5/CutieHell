﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AreasLinker : MonoBehaviour
{
    private bool opened;

	void Start ()
    {
        opened = false;
	}
	
	void Update ()
    {
		
	}

   public void SetOpened(bool isOpened)
    {
        if(opened != isOpened)
        {
            opened = isOpened;
            ActuateOnDoor();
        }
    }

    void ActuateOnDoor()
    {
        if (opened == true)
        {
            transform.position = transform.position + new Vector3(0, 15, 0);
        } else
        {
            transform.position = transform.position + new Vector3(0, -15, 0);
        }
        transform.GetComponent<NavMeshObstacle>().carving = true;
    }
}
