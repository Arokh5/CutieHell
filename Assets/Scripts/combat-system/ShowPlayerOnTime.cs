using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerOnTime : MonoBehaviour {

    public float timeToSpawn;
    private float timer = 0.0f;
    private bool flag = false;

    // Use this for initialization
    void OnEnable()
    {
        timer = 0.0f;
        flag = false;
    }

    // Update is called once per frame
    void Update () {
        if (!flag)
        {
            timer += Time.deltaTime;
            if (timer >= timeToSpawn)
            {
                GameManager.instance.GetPlayer1().SetRenderersVisibility(true);
                GameManager.instance.GetPlayer1().animator.SetTrigger("StrongAttack");
                flag = true;
            }
        }
    }
}
