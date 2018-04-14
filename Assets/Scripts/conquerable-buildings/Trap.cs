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
    private GameObject trapBasicSummonerEyes;
    public BeamProjection trapBasicSummonerBeam;
    public GameObject seductiveTrapActiveArea;
    public GameObject seductiveProjection;
    public float cooldownBetweenSeductiveProjections;
    private bool firstProjection;
    private GameObject nonLandedProjection;

    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject summonerTrapCamera;

    private List<GameObject> landedEnemyProjection =  new List<GameObject>();

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
            isActive = true;
            activate = false;
        }
        else if (deactivate)
        {
            isActive = false;
            deactivate = false;         
        }

        if (isActive)
        {
            if (trapType == TrapTypes.SEDUCTIVE)
            {
                LookAtSeductiveEnemyProjection();                
            }
        }

        if (landedEnemyProjection.Count > 0)
        {
            zoneController.EvaluateEnemiesTargettingProjections();
        }

        base.Update();
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, attractionRadius);
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
            if (player.state == Player.PlayerStates.SEDUCTIVE)
            {
                mainCamera.SetActive(false);
                summonerTrapCamera.SetActive(true);
                seductiveTrapActiveArea.SetActive(true);

                firstProjection = true;
                InstantiateSeductiveEnemyProjection();
                trapBasicSummonerBeam.gameObject.SetActive(true);                
            }
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
        if (player.state == Player.PlayerStates.SEDUCTIVE)
        {
            seductiveTrapActiveArea.SetActive(false);
            mainCamera.SetActive(true);
            summonerTrapCamera.SetActive(false);
            trapBasicSummonerBeam.gameObject.SetActive(false);
            GameObject.Destroy(nonLandedProjection);
        }

        player = null;
        deactivate = true;
        zoneController.OnTrapDeactivated();        
    }


    public void InstantiateSeductiveEnemyProjection()
    {
        Vector3 localStartPosition;

        // New generated projections (but not landed) will show up right where the player landed the previous one, first projection, exceptionally will land right in front of the trap
        if (firstProjection)
        {
            localStartPosition = transform.forward *
                        seductiveTrapActiveArea.GetComponent<Projector>().orthographicSize * 0.5f;
            
            Debug.Log("Hacer esto más limpio, en un metodo propio y pasar por el mismo sitio esta información aquí y a enemyprojection");
        }
        else
        {
            localStartPosition = nonLandedProjection.transform.localPosition;
        }

        nonLandedProjection = Instantiate(seductiveProjection,
            localStartPosition, Quaternion.LookRotation(transform.forward, transform.up), this.transform);
        nonLandedProjection.transform.localPosition = localStartPosition;

        nonLandedProjection.GetComponent<EnemyProjection>().SetLimitedPlacingDistance(seductiveTrapActiveArea.GetComponent<Projector>().orthographicSize * 0.7f);
        trapBasicSummonerBeam.enemyProjectionTargetPoint = nonLandedProjection.GetComponent<EnemyProjection>().headTransform;

        Debug.Log("Cambiar el getComponent");

    }

    public void LandSeductiveEnemyProjection()
    {
        landedEnemyProjection.Add(nonLandedProjection);
        landedEnemyProjection[GetLandedEnemyProjectionsCount()-1].GetComponent<EnemyProjection>().SetEnemyProjectionLanded(true);
        landedEnemyProjection[GetLandedEnemyProjectionsCount()-1].GetComponent<Renderer>().material.SetColor("_Color",Color.blue);

        zoneController.AddEnemyProjection(landedEnemyProjection[GetLandedEnemyProjectionsCount() - 1].GetComponent<EnemyProjection>());

        if (landedEnemyProjection.Count == 1)
        {
            firstProjection = false;
        }
    }

    public void LookAtSeductiveEnemyProjection()
    {
        trapBasicSummonerEyes.transform.rotation = Quaternion.LookRotation(nonLandedProjection.transform.position - trapBasicSummonerEyes.transform.position);
    }

    public int GetLandedEnemyProjectionsCount()
    {
        return landedEnemyProjection.Count;
    }

    public void DestroyEnemyProjection(GameObject deadEnemyProjection)
    {
        zoneController.RemoveEnemyProjection(deadEnemyProjection.GetComponent<EnemyProjection>());
        GameObject.Destroy(landedEnemyProjection[landedEnemyProjection.IndexOf(deadEnemyProjection)]);
        landedEnemyProjection.Remove(deadEnemyProjection);
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
    private void EraseAllLandedEnemyProjections()
    {       
        while (landedEnemyProjection.Count > 0)
        {
            GameObject.Destroy(landedEnemyProjection[0]);
            landedEnemyProjection.RemoveAt(0);
        }      
    }

    
    #endregion

}
