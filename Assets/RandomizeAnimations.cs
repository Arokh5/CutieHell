using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAnimations : MonoBehaviour {

    [SerializeField]
    private bool breakIdle;
    [SerializeField]
    private bool windExplosion;
    private Animator animator;
    private float nextBreakIdle;

    void Awake () {
        animator = this.GetComponent<Animator>();
        animator.SetFloat("Speed", Random.Range(0.8f, 1.2f));
        nextBreakIdle = Random.Range(4.0f, 9.0f);
	}

	void Update () {
        if (breakIdle)
        {
            nextBreakIdle -= Time.deltaTime;
            if(nextBreakIdle <= 0.0f)
            {
                nextBreakIdle = Random.Range(4.0f, 9.0f);
                animator.SetTrigger("BreakIdle");
            }
        }
	}
}
