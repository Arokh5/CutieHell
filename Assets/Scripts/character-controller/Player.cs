using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    [Header("Speed Variabes")]
    public float maxSpeed = 10;
    public float acceleration = 50;

    [Header("Evilness")]
    [SerializeField]
    private int maxEvilLevel = 20;
    [SerializeField]
    [ShowOnly]
    private int evilLevel;

    private Rigidbody rb;
    private Vector3 speedDirection;
    private GameObject[] traps;

    [Header("Actual Trap")]
    public GameObject actualTrap;

    [Header("Player States")]
    public PlayerStates state;
    public MeshRenderer meshRenderer;

    public enum PlayerStates { STILL, MOVE, WOLF, FOG, TURRET}

	void Start () 
    {
        evilLevel = maxEvilLevel;
        meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        rb = this.GetComponent<Rigidbody>();
        state = PlayerStates.MOVE;
        ResetTrapList();
    }

    private void Update() 
    {
        UseTrap();
    }

    private void FixedUpdate() 
    {
        switch (state) 
        {
            case PlayerStates.STILL:
                break;
            case PlayerStates.MOVE:
                MovePlayer();
                break;
            case PlayerStates.WOLF:
                break;
            case PlayerStates.FOG:
                break;
            case PlayerStates.TURRET:
                break;
            default:
                break;
        }
    }

    public void StopTrapUse() 
    {

    }

    private void MovePlayer() 
    {
        speedDirection = Vector3.zero;

        if (InputManager.instance.GetLeftStickUp()) {
            speedDirection += new Vector3(0.0f, 0.0f, 0.5f);
        }
        if (InputManager.instance.GetLeftStickDown()) {
            speedDirection += new Vector3(0.0f, 0.0f, -0.5f);
        }
        if (InputManager.instance.GetLeftStickLeft()) {
            speedDirection += new Vector3(-0.5f, 0.0f, 0.0f);
        }
        if (InputManager.instance.GetLeftStickRight()) {
            speedDirection += new Vector3(0.5f, 0.0f, 0.0f);
        }

        if (speedDirection.magnitude > 0.0f) {
            rb.drag = 0.0f;
        } else {
            rb.drag = 10.0f;
        }

        rb.AddRelativeForce(speedDirection * acceleration, ForceMode.Acceleration);

        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void UseTrap() 
    {
        if (actualTrap == null && state != PlayerStates.TURRET) 
        {
            for (int i = 0; i < traps.Length; i++) 
            {
                if (Vector3.Distance(this.transform.position, traps[i].transform.position) < 3.0f) 
                {
                    if (InputManager.instance.GetXButtonDown()) 
                    {
                        //Trap trapScript = traps[i].GetComponent<Trap>();
                        //if (trapScript.CanUse()) 
                        //{
                        //    this.transform.position = traps[i].transform.position;
                        //    trapScript.Activate(this);
                        //    nextState = PlayerStates.TURRET;
                        //}
                        this.transform.position = traps[i].transform.position;
                        meshRenderer.enabled = false;
                        actualTrap = traps[i];
                        //trapScript.Activate(this);
                        state = PlayerStates.TURRET;
                    }
                }
            }
        }
        else
        {
            if (InputManager.instance.GetXButtonDown()) 
            {
                meshRenderer.enabled = true;
                this.transform.position = actualTrap.transform.position - actualTrap.transform.forward * 3f;
                actualTrap = null;
                state = PlayerStates.MOVE;
            }
        }
    }

    public void ResetTrapList() 
    {
        traps = null;
        traps = GameObject.FindGameObjectsWithTag("Traps");
    }

    public int GetMaxEvilLevel() 
    {
        return maxEvilLevel;
    }

    public int GetEvilLevel()
    {
        return evilLevel;
    }

    public void SetEvilLevel(int value)
    {
        evilLevel += value;

        if (evilLevel < 0)
        {
            evilLevel = 0;
        }
        else if (evilLevel > maxEvilLevel)
        {
            evilLevel = maxEvilLevel;
        }

        UIManager.instance.SetEvilBarValue(evilLevel);
    }
}