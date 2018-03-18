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
    private Vector3 initialBulletSpawnPointPos;

    [Header("Actual Trap")]
    public GameObject actualTrap;

    [Header("Player States")]
    public PlayerStates state;
    public MeshRenderer meshRenderer;

    public enum PlayerStates { STILL, MOVE, WOLF, FOG, TURRET}

    private void Awake() 
    {
        state = PlayerStates.MOVE;
        initialBulletSpawnPointPos = new Vector3(0.8972f, 1.3626f, 0.1209f);
    }

    void Start () 
    {
        evilLevel = maxEvilLevel;
        meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        rb = this.GetComponent<Rigidbody>();
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
        Transform bulletSpawnPoint = actualTrap.transform.GetChild(0);
        bulletSpawnPoint.SetParent(transform);
        bulletSpawnPoint.localPosition = initialBulletSpawnPointPos;
        bulletSpawnPoint.rotation = Quaternion.identity;
        Trap trapScript = actualTrap.GetComponent<Trap>();
        trapScript.Deactivate();
        meshRenderer.enabled = true;
        Vector3 nextPos = actualTrap.transform.forward * 3f;
        this.transform.position = new Vector3(actualTrap.transform.position.x - nextPos.x, this.transform.position.y, actualTrap.transform.position.z - nextPos.z);
        actualTrap = null;
        state = PlayerStates.MOVE;
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
                        Trap trapScript = traps[i].GetComponent<Trap>();
                        if (trapScript.CanUse()) 
                        {
                            transform.GetChild(2).SetParent(traps[i].transform);
                            traps[i].transform.GetChild(0).localPosition = new Vector3(0f, 0.3f, 0.7f);
                            trapScript.Activate(this);
                            state = PlayerStates.TURRET;
                            meshRenderer.enabled = false;
                            actualTrap = traps[i];
                        }
                    }
                }
            }
        }
        else
        {
            if (InputManager.instance.GetXButtonDown()) 
            {
                StopTrapUse();
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