using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerTrap : Trap
{

    [Header("Seductive Trap setup")]
    [SerializeField]
    public GameObject trapBasicSummonerEyes;
    public BeamProjection trapBasicSummonerBeam;
    public Projector seductiveTrapActiveArea;
    public GameObject seductiveProjection;
    public float cooldownBetweenSeductiveProjections;

    public bool firstProjection = true;
    private GameObject nonLandedProjection;
    private EnemyProjection nonLandedProjectionScript;
    private List<GameObject> landedEnemyProjection = new List<GameObject>();

    public CameraController camera;

    #region MonoBehaviour Methods
    private new void Update()
    {
        base.Update();
        if (landedEnemyProjection.Count > 0)
        {
            zoneController.EvaluateEnemiesTargettingProjections();
        }
    }
    #endregion

    #region Public Methods
    public void InstantiateSeductiveEnemyProjection()
    {
        Vector3 localStartPosition;

        // New generated projections (but not landed) will show up right where the player landed the previous one, first projection, exceptionally will land right in front of the trap
        if (firstProjection)
        {
            localStartPosition = transform.forward * seductiveTrapActiveArea.orthographicSize * 0.5f;
        }
        else
        {
            localStartPosition = nonLandedProjection.transform.localPosition;
        }

        //Instantiate a new projection and located on the battlefield
        nonLandedProjection = Instantiate(seductiveProjection,
            localStartPosition, Quaternion.LookRotation(transform.forward, transform.up), this.transform);
        nonLandedProjection.transform.localPosition = localStartPosition;

        InitialSetUpForNewEnemyProjection();
    }

    public void LandSeductiveEnemyProjection()
    {
        landedEnemyProjection.Add(nonLandedProjection);
        landedEnemyProjection[GetLandedEnemyProjectionsCount() - 1].GetComponent<EnemyProjection>().SetEnemyProjectionLanded(true);
        landedEnemyProjection[GetLandedEnemyProjectionsCount() - 1].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

        zoneController.AddEnemyProjection(landedEnemyProjection[GetLandedEnemyProjectionsCount() - 1].GetComponent<EnemyProjection>());

        if (firstProjection == true)
        {
            firstProjection = false;
        }
    }

    public int GetLandedEnemyProjectionsCount()
    {
        return landedEnemyProjection.Count;
    }

    public GameObject GetNonLandedProjection()
    {
        return nonLandedProjection;
    }

    public void DestroyEnemyProjection(GameObject deadEnemyProjection)
    {
        zoneController.RemoveEnemyProjection(deadEnemyProjection.GetComponent<EnemyProjection>());
        GameObject.Destroy(landedEnemyProjection[landedEnemyProjection.IndexOf(deadEnemyProjection)]);
        landedEnemyProjection.Remove(deadEnemyProjection);
    }

    public List<AIEnemy> ObtainEnemiesAffectedByProjectionExplosion(Transform enemyProjectionTransform, float explosionRange)
    {
        List<AIEnemy> affectedEnemies = zoneController.DetectEnemiesWithinAnSpecificRange(enemyProjectionTransform, explosionRange);

        return affectedEnemies;
    }

    public List<EnemyProjection> ObtainEnemyProjectionsAffectedByProjectionExplosion(Transform enemyProjectionTransform, float explosionRange)
    {
        List<EnemyProjection> affectedEnemyProjections = zoneController.DetectEnemyProjectionsWithinAnSpecificRange(enemyProjectionTransform, explosionRange);

        return affectedEnemyProjections;
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

    private void InitialSetUpForNewEnemyProjection()
    { 
        nonLandedProjection.transform.localPosition = new Vector3(nonLandedProjection.transform.localPosition.x, -0.25f, nonLandedProjection.transform.localPosition.z); //Adjust y position 

        nonLandedProjectionScript = nonLandedProjection.GetComponent<EnemyProjection>();

        nonLandedProjectionScript.SetLimitedPlacingDistance(seductiveTrapActiveArea.orthographicSize * 0.7f); //Limit how far the enemyProjection can move
        trapBasicSummonerBeam.enemyProjectionTargetPoint = nonLandedProjectionScript.headTransform; // Beam showing the summoning effect
        camera.SetSummonedProjectionToFollow(nonLandedProjectionScript);
    }     
    #endregion
}
