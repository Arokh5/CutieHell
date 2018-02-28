using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScenariosManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.Find("Area1").Find("ConquerableStructure1").GetComponent<ConquerableElement>().SetConquered(!transform.Find("Area1").Find("ConquerableStructure1").GetComponent<ConquerableElement>().GetConquered()); //TODO substitute the hardcoded
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (transform.Find("Area1").Find("weakTrapArea1_1").GetComponent<ConquerableElement>().GetActive() == true)
            {
                transform.Find("Area1").Find("weakTrapArea1_1").GetComponent<ConquerableElement>().SetBeingUsed(true);
            }
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            if (transform.Find("Area1").Find("weakTrapArea1_1").GetComponent<ConquerableElement>().GetActive() == true)
            {
                transform.Find("Area1").Find("weakTrapArea1_1").GetComponent<ConquerableElement>().SetBeingUsed(false);
            }
        }
    }
}
