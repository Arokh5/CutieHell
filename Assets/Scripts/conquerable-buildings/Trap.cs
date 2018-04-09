using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Building, IUsable
{

    #region Fields
    [Header("Trap setup")]
    [SerializeField]
    private int trapID;
    public TrapTypes trapType;
    public int usageCost;
    private Player player;

    [Header("Seductive Trap setup")]
    [SerializeField]
    public GameObject seductiveTrapActiveArea;
    [SerializeField]
    public GameObject seductiveProjection;
    public float seductiveTrapDuration;
    public float blinkingSpeed;
    private float seductiveTrapCurrentTimeActive = 0;
    private float areaBlinkingTime = 0;
    private GameObject instantiatedEnemyProjection;

    [Header("Trap testing")]
    public bool activate = false;
    public bool deactivate = false;
    
    [ShowOnly]
    public bool isActive = false;
    #endregion

    public enum TrapTypes { TURRET, SEDUCTIVE }

    #region MonoBehaviour Methods
    private new void Update()
    {
        if (activate)
        {
            if(trapType == TrapTypes.SEDUCTIVE)
            {
                seductiveTrapActiveArea.SetActive(true);
                Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f) + transform.forward * transform.parent.transform.parent.GetComponentInChildren<Projector>().orthographicSize * 0.7f;
                Debug.Log("Hacer esto más limpio, en un metodo propio y pasar por el mismo sitio esta información aquí y a enemyprojection");
                startPosition.y = 0.0f;
                Debug.Log("Este último valor tiene que ser en función del ortographic size del SeductiveProjector");

                instantiatedEnemyProjection = Instantiate(seductiveProjection, startPosition, Quaternion.LookRotation(transform.forward, transform.up), transform.parent.transform);
                instantiatedEnemyProjection.transform.localPosition = startPosition;
                instantiatedEnemyProjection.AddComponent<EnemyProjection>();
            }
            isActive = true;
            activate = false;
        }
        else if (deactivate)
        {
            isActive = false;
            deactivate = false;
            Destroy(instantiatedEnemyProjection);
        }

        if (isActive)
        {
            if (trapType == TrapTypes.SEDUCTIVE)
            {
                HandleSeductiveAreaBlinking();
            }
        }
        base.Update();
    }
    #endregion

    #region Public Methods
    // IDamageable
    // If this method is called, it should inform the UIManager
    public override void FullRepair()
    {
        base.FullRepair();
        UIManager.instance.SetTrapConquerRate(zoneController.GetZoneId(), trapID, 0);
    }

    // IUsable
    // Called by Player
    public bool CanUse()
    {
        return currentHealth > 0;
    }

    // Called by Player
    public int GetUsageCost()
    {
        return usageCost;
    }

    // Called by Player. A call to this method should inform the ZoneController
    public bool Activate(Player player)
    {
        if (CanUse())
        {
            this.player = player;
            zoneController.OnTrapActivated(this);
            activate = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Called by Player. A call to this method should inform the ZoneController
    public void Deactivate()
    {
        if(player.state == Player.PlayerStates.SEDUCTIVE)
        {
            areaBlinkingTime = 0.0f;
            seductiveTrapActiveArea.SetActive(false);
            seductiveTrapCurrentTimeActive = 0.0f;
        }

        player = null;
        deactivate = true;
        zoneController.OnTrapDeactivated();     
    }
    #endregion

    #region Protected Methods
    protected override void BuildingKilled()
    {
        isActive = false;
        /* Trap could be killed by a Conqueror after the player got off */
        if (player != null)
        {
            player.StopTrapUse();
        }
    }

    protected override void InformUIManager()
    {
        float conquerRate = (baseHealth - currentHealth) / baseHealth;
        UIManager.instance.SetTrapConquerRate(zoneController.GetZoneId(), trapID, conquerRate);
    }
    #endregion

    #region Private Methods
    private void HandleSeductiveAreaBlinking()
    {
        if (seductiveTrapCurrentTimeActive < seductiveTrapDuration)
        {
            //Start Area blinking after 3/4 parts of the time has been consumed
            if (seductiveTrapCurrentTimeActive > (seductiveTrapDuration * 0.75))
            {
                if (seductiveTrapCurrentTimeActive > (seductiveTrapDuration * 0.9))
                {
                    areaBlinkingTime += blinkingSpeed * Time.deltaTime;
                }
                else
                {
                    areaBlinkingTime += blinkingSpeed * 2 * Time.deltaTime;
                }

                if (areaBlinkingTime > 1)
                {
                    seductiveTrapActiveArea.SetActive(!seductiveTrapActiveArea.activeSelf);
                    areaBlinkingTime = 0f;
                }

            }
            seductiveTrapCurrentTimeActive += Time.deltaTime;
        }
        else
        {
            player.StopTrapUse();
        }
    }
    #endregion

}
