using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Variabes")]
    public float maxSpeed = 10;
    public float acceleration = 50;
    [SerializeField]
    private Transform centerTeleportPoint;
    [SerializeField]
    private Transform statueTeleportPoint;

    [Header("Evilness")]
    [SerializeField]
    private int maxEvilLevel = 50;
    [SerializeField]
    [ShowOnly]
    private int evilLevel;

    private Rigidbody rb;
    private Vector3 speedDirection;
    private GameObject[] traps;
    private Vector3 initialBulletSpawnPointPos;
    private float timeSinceLastTrapUse;

    [Header("Attacks")]
    [SerializeField]
    public Transform bulletSpawnPoint;

    [Header("Actual Trap")]
    public GameObject actualTrap;
    [SerializeField]
    private int trapUseCooldown = 10;

 

    [Header("Player States")]
    public PlayerStates state;
    public MeshRenderer meshRenderer;

    public enum PlayerStates { STILL, MOVE, WOLF, FOG, TURRET, SEDUCTIVE }

    private void Awake()
    {
        state = PlayerStates.MOVE;
        initialBulletSpawnPointPos = new Vector3(0.8972f, 1.3626f, 0.1209f);
    }

    void Start()
    {
        evilLevel = maxEvilLevel;
        meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        rb = this.GetComponent<Rigidbody>();
        ResetTrapList();
        timeSinceLastTrapUse = trapUseCooldown;
    }

    private void Update()
    {
        UseTrap();

        if (InputManager.instance.GetL1ButtonDown() && state == PlayerStates.MOVE)
            transform.position = statueTeleportPoint.position;

        if (InputManager.instance.GetR1ButtonDown() && state == PlayerStates.MOVE)
            transform.position = centerTeleportPoint.position;

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

    private void PrepareTrapUse(Trap trapToUse)
    {
        Trap.TrapTypes trapType = trapToUse.trapType;
       
        switch (trapType)
        {
            case Trap.TrapTypes.TURRET:

                bulletSpawnPoint.SetParent(trapToUse.transform);
                bulletSpawnPoint.localPosition = new Vector3(0f, 0.3f, 0.7f);
                
                state = PlayerStates.TURRET;
                break;
            case Trap.TrapTypes.SEDUCTIVE:

                GameManager.instance.SetCrosshairActivate(false);
                state = PlayerStates.SEDUCTIVE;
                break;
        }
        trapToUse.Activate(this);
        actualTrap = trapToUse.gameObject;
        meshRenderer.enabled = false;
        GameManager.instance.SetTrapBeingUsed(actualTrap.GetComponent<Trap>());
    }

    public void StopTrapUse()
    {
        bulletSpawnPoint.SetParent(transform);
        bulletSpawnPoint.localPosition = initialBulletSpawnPointPos;
        bulletSpawnPoint.rotation = Quaternion.identity;
        Trap trapScript = actualTrap.GetComponent<Trap>();
        trapScript.Deactivate();
        meshRenderer.enabled = true;
        Vector3 nextPos = actualTrap.transform.forward * 3f;
        this.transform.position = new Vector3(actualTrap.transform.position.x - nextPos.x, this.transform.position.y, actualTrap.transform.position.z - nextPos.z);
        actualTrap = null;
        GameManager.instance.SetTrapBeingUsed(null);

        if (state == PlayerStates.SEDUCTIVE)
        {
            GameManager.instance.SetCrosshairActivate(true);
        }

        state = PlayerStates.MOVE;
        timeSinceLastTrapUse = 0f;
        
    }

    private void MovePlayer()
    {
        speedDirection = Vector3.zero;

        if (InputManager.instance.GetLeftStickUp())
        {
            speedDirection += new Vector3(0.0f, 0.0f, 0.5f);
        }
        if (InputManager.instance.GetLeftStickDown())
        {
            speedDirection += new Vector3(0.0f, 0.0f, -0.5f);
        }
        if (InputManager.instance.GetLeftStickLeft())
        {
            speedDirection += new Vector3(-0.5f, 0.0f, 0.0f);
        }
        if (InputManager.instance.GetLeftStickRight())
        {
            speedDirection += new Vector3(0.5f, 0.0f, 0.0f);
        }

        if (speedDirection.magnitude > 0.0f)
        {
            rb.drag = 0.0f;
        }
        else
        {
            rb.drag = 10.0f;
        }

        rb.AddRelativeForce(speedDirection * acceleration, ForceMode.Acceleration);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void UseTrap()
    {
        UIManager.instance.HideRepairTrapText();
        timeSinceLastTrapUse += Time.deltaTime;
        if (actualTrap == null && state != PlayerStates.TURRET)
        {
            if (timeSinceLastTrapUse > trapUseCooldown)
            {
                Trap trapScript = null;
                for (int i = 0; i < traps.Length; i++)
                {
                    if (Vector3.Distance(this.transform.position, traps[i].transform.position) < 3.0f)
                    {
                        trapScript = traps[i].GetComponent<Trap>();
                        break;
                    }
                }

                if (trapScript != null)
                {
                    if (InputManager.instance.GetXButtonDown())
                    {
                        if (trapScript.CanUse())
                        {
                            PrepareTrapUse(trapScript);
                        }
                    }
                    if (!trapScript.HasFullHealth() && trapScript.GetRepairCost() <= evilLevel)
                    {
                        UIManager.instance.ShowRepairTrapText();

                        if (InputManager.instance.GetTriangleButtonDown())
                        {
                            SetEvilLevel(-trapScript.GetRepairCost());
                            trapScript.FullRepair();
                            UIManager.instance.HideRepairTrapText();
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